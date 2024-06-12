using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

// This fixes the ambiguity between System.Random and UnityEngine.Random by
// telling it to use the Unity one.
using Random = UnityEngine.Random;


[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(StateMachine))]
[RequireComponent(typeof(StatusEffectsManager))]
public class Enemy_Base : MonoBehaviour, IEnemy
{
    public static bool ShowDistractednessBar = true;

    // This event is static so we don't need to subscribe to every enemy instance's OnEnemyDied event.
    public static event EventHandler OnEnemyDied;
    public static event EventHandler OnEnemyReachedGoal;



    [Tooltip("This enemy's stats information")]
    [SerializeField] protected EnemyInfo_Base _EnemyInfo;
   

    protected NavMeshAgent _NavMeshAgent;

    protected float _AttackDamage;
    protected float _BaseMovementSpeed;
    protected float _RewardAmount;
    protected float _WayPointArrivedDistance;

    protected float _Health; // This enemy's current health.
    protected bool _IsDead = false; // If this enemy has been defeated or not.

    protected float _DistanceFromNextWayPoint = 0f;
    protected WayPoint _NextWayPoint;

    private AudioSource enemyAudio;

    public List<GameObject> slowingEntities;
    public List<GameObject> stoppingEntities;
    public bool isATarget = false;

    private StateMachine _StateMachine;



    protected void Awake()
    {
        InitEnemyStats();

        IsDead = false;

        _NavMeshAgent = GetComponent<NavMeshAgent>();

        enemyAudio = GetComponent<AudioSource>();


        _StateMachine = GetComponent<StateMachine>();
        if (_StateMachine == null)
            throw new Exception($"The enemy \"{gameObject.name}\" does not have a StateMachine component!");

        InitStateMachine();
    }

    // Start is called before the first frame update
    protected void Start()    
    {
        // Find the closest WayPoint and start moving there.
        FindNearestWayPoint();

        _NavMeshAgent.SetDestination(_NextWayPoint.transform.position);
    }

    /// <summary>
    /// Initializes the stats for this enemy.
    /// Subclasses should override this function to init stats specific to that enemy type.
    /// </summary>
    protected virtual void InitEnemyStats()
    {
        _AttackDamage = _EnemyInfo.AttackDamage;
        _BaseMovementSpeed = _EnemyInfo.BaseMovementSpeed;
        _Health = _EnemyInfo.MaxHealth;
        _RewardAmount = _EnemyInfo.RewardAmount;
        _WayPointArrivedDistance = _EnemyInfo.WayPointArrivedDistance;
    }

    /// <summary>
    /// This function is overriden by subclasses to allow them to setup the state machine with their own states.
    /// </summary>
    protected virtual void InitStateMachine()
    {
        // Create enemy states.
        EnemyState_Idle_Base idleState = new EnemyState_Idle_Base(this);
        EnemyState_Dead_Base deadState = new EnemyState_Dead_Base(this);
        EnemyState_Moving movingState = new EnemyState_Moving(this);
        EnemyState_Slowed slowedState = new EnemyState_Slowed(this);
        EnemyState_Stopped stoppedState = new EnemyState_Stopped(this);


        // Create and register transitions.
        // ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        _StateMachine.AddTransitionFromState(movingState, new Transition(slowedState, () => slowingEntities.Count > 0 && stoppingEntities.Count == 0));
        _StateMachine.AddTransitionFromState(movingState, new Transition(stoppedState, () => stoppingEntities.Count > 0));
        _StateMachine.AddTransitionFromState(slowedState, new Transition(stoppedState, () => stoppingEntities.Count > 0));
        _StateMachine.AddTransitionFromState(stoppedState, new Transition(slowedState, () => stoppingEntities.Count == 0 &&
                                                                                             slowingEntities.Count > 0));

        _StateMachine.AddTransitionFromAnyState(new Transition(movingState, () => slowingEntities.Count == 0 && stoppingEntities.Count == 0));

        // If health is at or below 0, then switch to the dead state regardless of what state this enemy is currently in.        
        _StateMachine.AddTransitionFromAnyState(new Transition(deadState, () => _Health <= 0));

        // ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


        // Tell state machine to write in the debug console every time it exits or enters a state.
        _StateMachine.EnableDebugLogging = true;

        // This is necessary since we only have one state and no transitions for now.
        // Mouse over the AllowUnknownStates property for more info.
        _StateMachine.AllowUnknownStates = true;


        // Set the starting state.
        _StateMachine.SetState(idleState);
    }

    // Update is called once per frame
    void Update()
    {
        if (_Health <= 0 && !_IsDead)
        {
            IsDead = true;
            return;
        }
        if (_NextWayPoint != null)
        {
            _DistanceFromNextWayPoint = Vector3.Distance(transform.position, _NextWayPoint.transform.position);

            if (HasReachedDestination())
            {
                GetNextWaypoint();

                // Check for null in case we are already at the last WayPoint, as GetNextWayPoint()
                // returns null if there is no next WayPoint.
                if (_NextWayPoint != null)
                    _NavMeshAgent.SetDestination(_NextWayPoint.transform.position);
            }
        }
        else
        {
            _DistanceFromNextWayPoint = 0f;
        }

    }

    //I am intending this function to be called from either the tower or the projectile that the tower fires
    public void ApplyDamage(float damageValue, Tower_Base targetingTower)
    {

        _Health -= damageValue;

        if (_Health <= 0 && !_IsDead)
        {
            StartCoroutine(PlayDeathSound());

            if (targetingTower)
            {
                targetingTower.targets.Remove(this.gameObject);
            }

            KillEnemy(1);
        }
    }

    protected void OnTriggerEnter(Collider other)
    {
        // Did this enemy reach the goal?
        if (other.gameObject.CompareTag("Goal"))
        {           
            KillEnemy(2);
        }
    }
    protected void KillEnemy(int type)
    {
        if (IsDead)
            return;


        // Prevents this function from running twice in rare cases, causing this cat's death to count as more than one.
        IsDead = true;
        if(type == 1)
        {
            // Fire the OnCatDied event.
            GameManager.Instance.MoneySystem.AddCurrency(_EnemyInfo.CurrencyGain);
            OnEnemyDied?.Invoke(this, EventArgs.Empty);
        }
        else if(type == 2)  
        {
            OnEnemyReachedGoal?.Invoke(this, EventArgs.Empty);
        }

        
        // Destroy this enemy.
        Destroy(gameObject);

    }

    protected void GetNextWaypoint()
    {
        int count = _NextWayPoint.NextWayPoints.Count;

        if (count == 0)
        {
            _NextWayPoint = null;
        }
        else if (count == 1)
        {
            _NextWayPoint = _NextWayPoint.NextWayPoints[0];
        }
        else // count is greater than 1
        {
            // The current waypoint has multiple next waypoints, so we will
            // select one at random.
            _NextWayPoint = _NextWayPoint.NextWayPoints[Random.Range(0, count)];
        }

    }

    protected void FindNearestWayPoint()
    {
        float minDistance = float.MaxValue;
        WayPoint nearestWayPoint = null;

        foreach (WayPoint wayPoint in FindObjectsByType<WayPoint>(FindObjectsSortMode.None))
        {
            float distance = Vector3.Distance(transform.position, wayPoint.transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                nearestWayPoint = wayPoint;
            }
        }


        _NextWayPoint = nearestWayPoint;
    }

    public bool HasReachedDestination()
    {
        return _DistanceFromNextWayPoint <= _WayPointArrivedDistance &&
               _NavMeshAgent.pathStatus == NavMeshPathStatus.PathComplete;
    }



    IEnumerator PlayDeathSound()
    {
        _NavMeshAgent.speed = 0;

        // TODO: Play death sound here

        yield return new WaitForSeconds(0.5f);

        KillEnemy(1);        
    }
   


    public float AttackDamage { get { return _EnemyInfo.AttackDamage; } }
    public float BaseMovementSpeed { get { return _BaseMovementSpeed; } }
    public EnemyTypes EnemyType { get { return _EnemyInfo.Type; } }    
    public float Health { get { return _Health; } }
    public bool IsBacteria { get; protected set; } = false;
    public bool IsDead { get; protected set; } = false;
    public bool IsFungi { get; protected set; } = false;
    public bool IsVirus { get; protected set; } = false;
    public float MaxHealth { get { return _EnemyInfo.MaxHealth; } }
    public float RewardAmount { get { return _EnemyInfo.RewardAmount; } }

    public WayPoint NextWayPoint 
    { 
        get { return _NextWayPoint; } 
        set
        { 
            _NextWayPoint = value; 
        }
    }
}
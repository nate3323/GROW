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


[RequireComponent(typeof(StateMachine))]
public class EnemyBase : MonoBehaviour, IEnemy
{
    public static bool ShowDistractednessBar = true;

    // This event is static so we don't need to subscribe to every enemy instance's OnEnemyDied event.
    public static event EventHandler OnEnemyDied;
    public static event EventHandler OnEnemyReachedGoal;



    [Tooltip("This enemy's type")]
    [SerializeField] protected EnemyTypes _EnemyType;

    [Min(0)]
    [SerializeField] protected float _MaxHealth = 50; //The max amount of health this enemy can have

    [Min(0f)]
    [SerializeField] protected float _AttackDamage = 2f;


    [Header("Enemy Movement")]

    [Tooltip("This emeny's base movement speed")]
    [Min(0f)]
    protected float _BaseMoveSpeed;

    [Tooltip("How much money to player gets for destoying this enemy.")]
    [SerializeField] protected float _RewardAmount = 50;

    [Min(0f)]
    [Tooltip("This sets how close the cat must get to the next WayPoint to consider itself to have arrived there. This causes it to then target the next WayPoint (or a randomly selected one if the current WayPoint has multiple next points set in the Inspector.")]
    [SerializeField] protected float _WayPointArrivedDistance = 2f;
    
    [Tooltip("Controls the enemies's navigation")]
    public NavMeshAgent agent;



    protected float _Health; // This enemy's current health.
    protected bool _IsDead = false; // If this enemy has been defeated or not.

    protected float _DistanceFromNextWayPoint = 0f;
    protected WayPoint _NextWayPoint;

    protected GameObject _DistractednessMeterGO;
    protected UnityEngine.UI.Image _DistractednessMeterBarImage;
    protected TextMeshPro _DistractednessMeterLabel;

    private AudioSource enemyAudio;

    public bool isATarget = false;

    private StateMachine _stateMachine;


    protected void Awake()
    {
        
    }

    // Start is called before the first frame update
    protected void Start()    
    {
        IsDead = false;
        enemyAudio = GetComponent<AudioSource>();

        agent = GetComponent<NavMeshAgent>();


        // Find the closest WayPoint and start moving there.
        //FindNearestWayPoint();
        //agent.SetDestination(_NextWayPoint.transform.position);


        if (_stateMachine == null)
        {
            _stateMachine = GetComponent<StateMachine>();
            if (_stateMachine == null)
                throw new Exception($"The enemy \"{gameObject.name}\" does not have a StateMachine component!");

            InitStateMachine();
        }
    }

    /// <summary>
    /// This function is overriden by subclasses to allow them to setup the state machine with their own states.
    /// </summary>
    protected virtual void InitStateMachine()
    {
        // Create enemy states.
        EnemyState_Idle_Base idleState = new EnemyState_Idle_Base(this);
        EnemyState_Dead_Base deadState = new EnemyState_Dead_Base(this);


        // Create and register transitions.
        // ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        //_stateMachine.AddTransitionFromState(movingState, new Transition(slowedState, () => slowingEntities.Count > 0 && stoppingEntities.Count == 0));

        // If health is at or below 0, then switch to the dead state regardless of what state this enemy is currently in.        
        _stateMachine.AddTransitionFromAnyState(new Transition(deadState, () => _Health <= 0));

        // ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


        // Tell state machine to write in the debug console every time it exits or enters a state.
        //_stateMachine.EnableDebugLogging = true;

        // This is necessary since we only have one state and no transitions for now.
        // Mouse over the AllowUnknownStates property for more info.
        _stateMachine.AllowUnknownStates = true;


        // Set the starting state.
        _stateMachine.SetState(idleState);
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
                    agent.SetDestination(_NextWayPoint.transform.position);
            }
        }
        else
        {
            _DistanceFromNextWayPoint = 0f;
        }


        _DistractednessMeterGO.SetActive(ShowDistractednessBar);
    }
    
    //I am intending this function to be called from either the tower or the projectile that the tower fires
    public void ApplyDamage(float damageValue, Tower targetingTower)
    {

        _Health -= damageValue;

        if (_Health <= 0 && !_IsDead)
        {
            StartCoroutine(PlayDeathSound());

            targetingTower.targets.Remove(this.gameObject);                       
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
               agent.pathStatus == NavMeshPathStatus.PathComplete;
    }



    IEnumerator PlayDeathSound()
    {
        agent.speed = 0;

        // TODO: Play death sound here

        yield return new WaitForSeconds(0.5f);

        KillEnemy(1);        
    }
   


    public float AttackDamage { get { return _AttackDamage; } }
    public float BaseMovementSpeed { get { return _BaseMoveSpeed; } }
    public EnemyTypes EnemyType { get { return _EnemyType; } }    
    public float Health { get { return _Health; } }
    public bool IsBacteria { get; protected set; } = false;
    public bool IsDead { get; protected set; } = false;
    public bool IsFungi { get; protected set; } = false;
    public bool IsVirus { get; protected set; } = false;
    public float MaxHealth { get { return _MaxHealth; } }
    public float RewardAmount { get { return _RewardAmount; } }
public WayPoint NextWayPoint 
    { 
        get { return _NextWayPoint; } 
        set
        { 
            _NextWayPoint = value; 
        }
    }
}
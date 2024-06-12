using System;
using System.Collections;

using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class SpawnedUnit : MonoBehaviour
{

    [Tooltip("This spawned unit's stats information")]
    [SerializeField] protected SpawnedUnitInfo_Base _SpawnedUnitInfo;


    protected NavMeshAgent _NavMeshAgent;

    protected float _AttackDamage;
    protected float _AttackSpeed;
    protected float _BaseMovementSpeed;
    protected float _RewardAmount;
    protected float _WayPointArrivedDistance;

    protected float _Health; // This enemy's current health.

    protected float _DistanceFromNextWayPoint = 0f;
    protected WayPoint _NextWayPoint;

    private Tower_Base tower;
    private bool isAttacking = false;
    private Enemy_Base target;

    public event EventHandler UnitDied;

    protected virtual void OnUnitDied(EventArgs e)
    {
        UnitDied?.Invoke(this, e);
    }
    void Start()
    {
        _NavMeshAgent = GetComponent<NavMeshAgent>();
        _NavMeshAgent.speed = _BaseMovementSpeed;
        tower = transform.parent.gameObject.GetComponent<Tower_Base>();

    }

    void Update()
    {

        //If the person has a target that is not in the list, the person does not have a target
        if (target != null && !tower.targets.Contains(target.gameObject))
        {
            RemoveTarget();

        }
        //If there are cats in range and the person does not have a target, find a target
        if (tower.targets.Count > 0 && target == null)
        {
            FindClosestAvailableEnemy();
        }
        if (target != null && tower.targets.Contains(target.gameObject))
        {
            _NavMeshAgent.SetDestination(target.transform.position);
            if(!(target.stoppingEntities.Count > 0) && !isAttacking)
            {
                target.stoppingEntities.Add(gameObject);
                isAttacking = true;
                OnAttack();
            }
        }
    }

    //Gets the enemy closest to the player that is not stopped and sets it as the target
    private void FindClosestAvailableEnemy()
    {
        GameObject closestEnemy = null;
        float smallestDist = 2000000;
        foreach (GameObject enemy in tower.targets)
        {
            if (enemy != null) //If the cat exists
            {
                if (EnemyDistance(enemy) < smallestDist &&                 //If it is closer to the person than the previous smallest distance
                    !(enemy.GetComponent<Enemy_Base>().IsATarget) &&       //If the enemy is not a target of another person
                    tower.targets.Contains(enemy))                         //If the enemy is in range of the tower  
                {
                    smallestDist = EnemyDistance(enemy);
                    closestEnemy = enemy;
                }
            }

        }
        target = closestEnemy.GetComponent<Enemy_Base>(); //Sets the target to the closest enemy
        if (target != null)
        {
            target.GetComponent<Enemy_Base>().SetAsTarget(this);
            _NavMeshAgent.SetDestination(target.transform.position);
        }
    }
    //Finds the distance between the person and the cat
    private float EnemyDistance(GameObject enemy)
    {
        float distance = 0f;
        float xDist = Mathf.Pow(transform.position.x - enemy.transform.position.x, 2);
        float yDist = Mathf.Pow(transform.position.x - enemy.transform.position.x, 2);
        distance = Mathf.Sqrt(xDist + yDist);
        return distance;
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
            _NextWayPoint = _NextWayPoint.NextWayPoints[UnityEngine.Random.Range(0, count)];
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

    public void RemoveTarget()
    {
        StopAllCoroutines();
        if (target != null)
        {
            target.SetNotTarget();
            target.stoppingEntities.Remove(gameObject);
            target = null;

        }
        isAttacking = false;
    }

    public void ApplyDamage(float damage)
    {
        _Health -= damage;
        if(_Health < 0f)
        {
            RemoveTarget();
            // Fire the OnUnitDied event.
            if( _Health <= 0 )
            {
                UnitDied?.Invoke(this, EventArgs.Empty);
                Destroy(gameObject);
            }
        }
    }

    IEnumerator OnAttack()
    {
        target.ApplyDamage(_AttackDamage, tower);
        _Health -= target.AttackDamage;
        yield return new WaitForSeconds(_AttackSpeed);
    }


}
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class SpawnedUnit : MonoBehaviour
{

    // Variables
    [Header("Unit Information")]
    [SerializeField, Tooltip("The starting health of the spawned unit")]
    private float health;
    [SerializeField, Tooltip("The attack speed of the spawned unit")]
    private float attackSpeed;
    [SerializeField, Tooltip("The attack damage of the spawned unit")]
    private float damage;
    [SerializeField, Tooltip("The movement of the spawned unit")]
    private float movementSpeed;

    private NavMeshAgent agent;
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
        agent = GetComponent<NavMeshAgent>();
        agent.speed = movementSpeed;
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
            agent.SetDestination(target.transform.position);
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
            agent.SetDestination(target.transform.position);
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
        health -= damage;
        if(health < 0f)
        {
            RemoveTarget();
            // Fire the OnUnitDied event.
            if( health <= 0 )
            {
                UnitDied?.Invoke(this, EventArgs.Empty);
                Destroy(gameObject);
            }
        }
    }

    IEnumerator OnAttack()
    {
        target.ApplyDamage(damage, tower);
        health -= target.AttackDamage;
        yield return new WaitForSeconds(attackSpeed);
    }


}
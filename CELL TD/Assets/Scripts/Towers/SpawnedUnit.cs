using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class SpawnedUnit : MonoBehaviour
{
    // Variables
    private NavMeshAgent agent;
    private Tower parentTower;
    private bool isAttacking = false;
    private GameObject target;
    private List<GameObject> targetsInRange;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        parentTower = transform.parent.gameObject.GetComponent<Tower>();
        targetsInRange = new List<GameObject>();

    }

    void Update()
    {

        //If there are cats in range and the person does not have a target, find a target
        if (parentTower.targets.Count > 0 && target == null)
        {
            //FindClosestAvailableCat();
        }
        if (target != null && parentTower.targets.Contains(target))
        {
            agent.SetDestination(target.transform.position);
        }
        //If the person has a target that is not in the list, the person does not have a target
        if (target != null && !parentTower.targets.Contains(target))
        {
            //RemoveTarget();

        }
        if (targetsInRange.Contains(target) && !(target.GetComponent<Enemy_Base>().stoppingEntities.Count > 0))
        {
            target.GetComponent<Enemy_Base>().stoppingEntities.Add(gameObject);

            
        }
    }

}
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;


/// <summary>
/// This is the Moving State providing functionality for Cats that are moving
/// </summary>
public class EnemyState_Stopped : EnemyState_Base
{
    public EnemyState_Stopped(Enemy_Base parent)
        : base(parent)
    {

    }

    public override void OnEnter()
    {
        _parent.gameObject.GetComponent<NavMeshAgent>().speed = 0;
        Debug.Log("Speed should be zero");
    }

    public override void OnExit()
    {
        _parent.gameObject.GetComponent<NavMeshAgent>().speed = _parent.GetComponent<Enemy_Base>().MovementSpeed;
        Debug.Log("Speed should be regular");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

}
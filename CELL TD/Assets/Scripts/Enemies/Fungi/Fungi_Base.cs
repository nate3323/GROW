using System.Collections;
using System.Collections.Generic;

using UnityEngine;


/// <summary>
/// This is the base class for all fungi-type enemies
/// </summary>
public class Fungi_Base : Enemy_Base, IFungi
{
    new void Awake()
    {
        base.Awake();

        // Do initialization here.
        IsFungi = true;
    }

    new void Start()
    {
        base.Start();

        // Do initialization here.
    }

    /// <summary>
    /// Initializes stats specific to fungi-type enemies.
    /// Stats common to all enemy types should be initialized in the base class version of this method.
    /// This function is called by the base class.
    /// </summary>
    protected override void InitEnemyStats()
    {
        base.InitEnemyStats();

        // Init fungi-specific enemy stats here.
    }

    /// <summary>
    /// Initializes the state machine of this enemy.
    /// This function is called by the base class.
    /// </summary>
    protected override void InitStateMachine()
    {
        // This probably isn't needed.
        //base.InitStateMachine();
    }
}

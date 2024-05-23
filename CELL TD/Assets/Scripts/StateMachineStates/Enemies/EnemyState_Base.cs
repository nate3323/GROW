
using UnityEngine;


/// <summary>
/// This is the base class that all state machine states have at the base of their inheritance heirarchy.
/// </summary>
public abstract class EnemyState_Base : State_Base
{
    protected EnemyBase _parent;



    /// <summary>
    /// The constructor.
    /// </summary>
    /// <param name="parent">The state needs a reference to its parent cat so it can call methods on it.</param>
    public EnemyState_Base(EnemyBase parent)
        : base(parent.gameObject)
    {
        _parent = parent;
    }


}


using System.Collections;
using System.Collections.Generic;

using UnityEngine;


/// <summary>
/// This state is for initialization when the game first starts up.
/// </summary>
public class GameState_StartUp : GameState_Base
{
    private bool _InitializationComplete;



    public GameState_StartUp(GameManager parent)
        : base(parent)
    {

    }


    public override void OnEnter()
    {
        _InitializationComplete = true;
    }

    public override void OnExit()
    {

    }

    public override void OnUpdate()
    {
        if (_InitializationComplete)
            _parent.NotifyInitializationCompleted();
    }
}

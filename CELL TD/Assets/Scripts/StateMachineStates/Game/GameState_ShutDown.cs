using System.Collections;
using System.Collections.Generic;

using UnityEngine;


/// <summary>
/// This state is for initialization when the game first starts up.
/// </summary>
public class GameState_ShutDown : GameState_Base
{
    bool _ShutdownIsComplete;


    public GameState_ShutDown(GameManager parent)
        : base(parent)
    {

    }


    public override void OnEnter()
    {
        // Do shutdown work.
        DoShutDownWork();

        // Set flag to indicate that shutdown work is complete.
        _ShutdownIsComplete = true;
    }

    public override void OnExit()
    {

    }

    public override void OnUpdate()
    {
        if (_ShutdownIsComplete)
        {
            Application.Quit();
        }
    }

    /// <summary>
    /// Handles any shutdown/cleanup logic.
    /// </summary>
    private void DoShutDownWork()
    {

    }
    
}

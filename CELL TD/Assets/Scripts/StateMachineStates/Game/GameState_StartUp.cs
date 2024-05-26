using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;


/// <summary>
/// This state is for initialization when the game first starts up.
/// </summary>
public class GameState_StartUp : GameState_Base
{
    private bool _StartUpIsComplete;

    private float _Timer;
    private bool _AnyButtonPressed;



    public GameState_StartUp(GameManager parent)
        : base(parent)
    {
        // Listen for any user input.
        InputSystem.onAnyButtonPress.Call(ctrl => OnAnyButtonPress(ctrl));
    }


    public override void OnEnter()
    {
        // Do startup work.
        DoStartUpWork();

        // Set flag to indicate that startup work is complete.
        _StartUpIsComplete = true;
    }

    public override void OnExit()
    {

    }

    public override void OnUpdate()
    {
        // Switch to main menu after 5 seconds for now.
        _Timer += Time.deltaTime;
        if (_StartUpIsComplete &&
            (_Timer >= 5.0f || _AnyButtonPressed))
        {
            _parent.NotifyInitializationCompleted();
        }
    }

    /// <summary>
    /// Handles any startup logic.
    /// </summary>
    private void DoStartUpWork()
    {

    }

    private void OnAnyButtonPress(InputControl control)
    {
        _AnyButtonPressed = true;
    }
}

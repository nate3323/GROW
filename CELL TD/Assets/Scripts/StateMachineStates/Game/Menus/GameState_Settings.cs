using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// This state is when the player enters the settings.
/// </summary>
public class GameState_Settings : GameState_Base
{
    public GameState_Settings(GameManager parent)
        : base(parent)
    {

    }


    public override void OnEnter()
    {        
        // Display the settings window
    }

    public override void OnExit()
    {
        // Close the settings window
    }

    public override void OnUpdate()
    {

    }
}

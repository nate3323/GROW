using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// This state is when the player is in the game, and not paused.
/// </summary>
public class GameState_InGame : GameState_Base
{
    public GameState_InGame(GameManager parent)
        : base(parent)
    {

    }


    public override void OnEnter()
    {
        SceneManager.LoadScene($"Level_{GameManager.Instance.CurrentLevelNumber}");
    }

    public override void OnExit()
    {

    }

    public override void OnUpdate()
    {

    }
}

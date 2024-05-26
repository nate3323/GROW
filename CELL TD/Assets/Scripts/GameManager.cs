using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


/// <summary>
/// This script manages the current game state. It is setup as a singleton so it can be easily accessed
/// from anywhere in the project.
/// </summary>
[RequireComponent(typeof(StateMachine))]

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    private StateMachine _StateMachine;



    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("There is already a GameManager in this scene. Self destructing...");
            Destroy(gameObject);
            return;
        }


        Instance = this;

        DontDestroyOnLoad(gameObject);


        HealthSystem = GameObject.Find("PlayerUI").GetComponent<HealthSystem>();
        MoneySystem = GameObject.Find("MoneyUI").GetComponent<MoneySystem>();


        _StateMachine = GetComponent<StateMachine>();
        if (_StateMachine == null)
            throw new Exception($"The GameManager game object \"{gameObject.name}\" does not have a StateMachine component!");

        InitStateMachine();
    }

    /// <summary>
    /// This function is overriden by subclasses to allow them to setup the state machine with their own states.
    /// </summary>
    protected virtual void InitStateMachine()
    {
        // Create startup and menu states
        GameState_StartUp startUpState = new GameState_StartUp(this);
        GameState_MainMenu mainMenuState = new GameState_MainMenu(this);
        GameState_PauseMenu pauseMenuState = new GameState_PauseMenu(this);

        // Create game states
        GameState_InGame inGameState = new GameState_InGame(this);
        GameState_Victory victoryState = new GameState_Victory(this);
        GameState_Defeat defeatedState = new GameState_Defeat(this);


        // Create and register automatic state transitions. The current state can also be changed manually via _StateMachine.SetState() if necessary.
        // ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        // Transition from startup to main menu when initialization has completed.        
        _StateMachine.AddTransitionFromState(startUpState, new Transition(mainMenuState, () => IsInitialized));

        // If health is at or below 0, then switch to the dead state regardless of what state this enemy is currently in.        
        //_StateMachine.AddTransitionFromAnyState(new Transition(deadState, () => HealthSystem.HealthAmount <= 0));

        // ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


        // Tell state machine to write in the debug console every time it exits or enters a state.
        _StateMachine.EnableDebugLogging = true;

        // This is necessary since we only have one state and no transitions for now.
        // Mouse over the AllowUnknownStates property for more info.
        _StateMachine.AllowUnknownStates = true;


        // Set the starting state.
        _StateMachine.SetState(startUpState);
    }

    public void NotifyInitializationCompleted()
    {
        IsInitialized = true;
    }



    public bool IsInitialized { get; private set; }

    public HealthSystem HealthSystem { get; private set; }
    public MoneySystem MoneySystem { get; private set; }
}

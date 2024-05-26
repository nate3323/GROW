using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;



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
            //Debug.LogWarning("There is already a GameManager in this scene. Self destructing...");
            Destroy(gameObject);
            return;
        }


        Instance = this;

        DontDestroyOnLoad(gameObject);

        SceneManager.activeSceneChanged += OnActiveSceneChanged;


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
        GameState_StartUp startUpState = new GameState_StartUp(this); // startup / init / title screen state
        GameState_ShutDown shutDownState = new GameState_ShutDown(this); // shutdown / cleanup state
        GameState_MainMenu mainMenuState = new GameState_MainMenu(this);
        GameState_PauseMenu pauseMenuState = new GameState_PauseMenu(this);
        GameState_LevelSelect levelSelectState = new GameState_LevelSelect(this);
        GameState_Settings settingsState = new GameState_Settings(this);

        // Manually register startup/menu states so they can be triggered via GameManager.SetGameState().
        // If we don't do this, the state machine will not know about any states that aren't referenced
        // by the transitions we define below.
        _StateMachine.AddStates(startUpState,
                                shutDownState,
                                mainMenuState,
                                pauseMenuState,
                                levelSelectState,
                                settingsState);

        // Create in-game states
        GameState_InGame inGameState = new GameState_InGame(this);
        GameState_Victory victoryState = new GameState_Victory(this);
        GameState_Defeat defeatedState = new GameState_Defeat(this);

        // Manually register in-game states.
        _StateMachine.AddStates(inGameState,
                                victoryState,
                                defeatedState);
        

        // Create and register automatic state transitions. The current state can also be changed manually via _StateMachine.SetState() if necessary.
        // ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        // Transition from startup to main menu when initialization has completed.        
        _StateMachine.AddTransitionFromState(startUpState, new Transition(mainMenuState, () => IsInitialized));

        // If health is at or below 0, then switch to the dead state regardless of what state this enemy is currently in.        
        //_StateMachine.AddTransitionFromAnyState(new Transition(deadState, () => HealthSystem.HealthAmount <= 0));

        // ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


        // Tell state machine to write in the debug console every time it exits or enters a state.
        _StateMachine.EnableDebugLogging = true;

        // Mouse over the AllowUnknownStates property for more info.
        _StateMachine.AllowUnknownStates = true;

        // Set the initial state of the state machine.
        _StateMachine.SetState(startUpState);
    }

    public void NotifyInitializationCompleted()
    {
        IsInitialized = true;
    }

    /// <summary>
    /// This function is called when we switch scenes to reacquire references to things like
    /// player UI, etc.
    /// </summary>
    public void UpdateReferences()
    {
        GameObject playerUI = GameObject.Find("PlayerUI");
        HealthSystem = playerUI != null ? playerUI.GetComponent<HealthSystem>()
                                        : null;
        
        GameObject moneyUI = GameObject.Find("MoneyUI");
        MoneySystem = moneyUI != null ? moneyUI.GetComponent<MoneySystem>()
                                        : null;

    }

    /// <summary>
    /// Called when the active scene changes.
    /// </summary>
    /// <param name="current">The outgoing scene.</param>
    /// <param name="next">The new active scene.</param>
    private void OnActiveSceneChanged(Scene current, Scene next)
    {
        UpdateReferences();
    }

    /// <summary>
    /// Sets the current state to the state that has the specified type.
    /// </summary>
    /// <param name="stateClassName"></param>
    public void SetGameState(Type stateType)
    {
        _StateMachine.SetState(stateType);       
    }

    

    public bool IsInitialized { get; private set; }

    public int CurrentLevelNumber { get; set; }
    public HealthSystem HealthSystem { get; private set; }
    public MoneySystem MoneySystem { get; private set; }
}

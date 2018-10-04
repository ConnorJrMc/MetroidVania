using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class handles switching between states, and acts as the interface for stats
public class StateMachine : MonoBehaviour {

    //Stack<State> states;


    private Stack<State> states;
    [SerializeField]
    State activeState;

    [SerializeField]
    State defaultState;

    [SerializeField]
    State[] AllStates;

    [SerializeField]
    PlayerCharacter Controller;
	// Use this for initialization
	void Start () {
		
	}

    void Awake()
    {
        states = new Stack<State>();
       

        foreach(State stt in AllStates)
        {
            stt.Activated();
        }

        //handle initialization of dynamic states
        
        //add the default state to the states list
        if (defaultState)
        PushState(defaultState, Controller);
        activeState = states.Peek();
    }
	
	// Update is called once per frame
	void Update () {
        activeState.HandleInput(Controller, this);
        activeState.HandleUpdate(Controller,this);
    }

    void FixedUpdate()
    {
        activeState.HandleFixedUpdate(Controller);
    }


    //end of the tick check if we need to change states
    void LateUpdate()
    {
    }

    public State GetCurrentState()
    {
        return activeState;
    }

    //push a new state to the stack
    public bool PushState(State newState, PlayerCharacter Controller)
    {
        if (!newState)
            return false;
        //cant push the same state
        if (newState == activeState)
            return false;

        //if there is an active state, exit that state
        if(activeState != null)
        {
            activeState.Exit(Controller);
        }

        newState.Enter(Controller);
        states.Push(newState);

        activeState = states.Peek();

        return true;
    }
    
    //remove a state from the stack
    public bool PopState(State oldState, PlayerCharacter Controller)
    {
        //if there is no  active state, then dont pop anything
        if (activeState == null)
            return false;
        //dont remove the state if they dont match
        if (oldState != activeState)
            return false;

        //remove the state
        activeState.Exit(Controller);
        states.Pop();
        //return to the next state in line
        activeState = states.Peek();

        //if we popped all of the states, default to our defualt state
        if (activeState == null)
            PushState(defaultState, Controller);
        else
            activeState.Enter(Controller);

        return true;
    }

    //clear our stack and set the new state to be the only active state
    public void ForceState(State newState, PlayerCharacter Controller)
    {
        if (activeState)
            activeState.Exit(Controller);

        states.Clear();

        newState.Enter(Controller);
        states.Push(newState);
        activeState = states.Peek();
    }
}

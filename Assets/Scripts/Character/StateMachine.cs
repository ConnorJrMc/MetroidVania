using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct StateDictionary
{
    [SerializeField]
    public string stateName;
    [SerializeField]
    public State state;
}


//class handles switching between states, and acts as the interface for stats
public class StateMachine : MonoBehaviour {

    //Stack<State> states;

    [SerializeField]
    StateDictionary[] dictionary;

   private Hashtable possibleStates;

    private Stack<State> states;
    [SerializeField]
    State activeState;

    [SerializeField]
    State defaultState;

    List<State> newStates;

    [SerializeField]
    PlayerCharacter Controller;
	// Use this for initialization
	void Start () {
		
	}

    //converst our list into a hashtable for easy look up later
    void BuildHashTable()
    {
        possibleStates = new Hashtable();

        foreach(StateDictionary dic in dictionary)
        {
            if (possibleStates.ContainsKey(dic.stateName))
                continue;

            possibleStates.Add(dic.stateName, dic.state);
        }
    }

    void Awake()
    {
        BuildHashTable();

        states = new Stack<State>();
        newStates = new List<State>();

        //handle initialization of dynamic states
        
        //add the default state to the states list
        if (defaultState)
        PushState(defaultState, Controller);
        activeState = states.Peek();
    }
	
	// Update is called once per frame
	void Update () {
        activeState.HandleInput(Controller);
        activeState.HandleUpdate(Controller);
    }

    void FixedUpdate()
    {
        activeState.HandleFixedUpdate(Controller);
    }
    //end of the tick check if we need to change states
    void LateUpdate()
    {
        ChangeStates();
    }
    void ChangeStates()
    {
        //iterate through the possible state changes that have occured
        //the highest value state is the state that is changed too
        State bestState = null;

        foreach(State possibleState in newStates)
        {
            if(!bestState)
            {
                bestState = possibleState;
                continue;
            }
            if(possibleState && possibleState.PriorityLevel > bestState.PriorityLevel)
            {
                bestState = possibleState;
            }
        }

        if (bestState)
            PushState(bestState, Controller);

    }

    public State GetCurrentState()
    {
        return activeState;
    }

    public void AddNewState(State newState)
    {
        //dont add duplicate states
        if (newStates.Contains(newState))
            return;

        newStates.Add(newState);
    }

    //push a new state to the stack
    bool PushState(State newState, PlayerCharacter Controller)
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
    bool PopState(State oldState, PlayerCharacter Controller)
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

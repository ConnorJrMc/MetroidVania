using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : ScriptableObject
{

    public string GetName()
    { return GetType().ToString(); }

   public void Activated()
    {
        BuildHashTable();
    }

    //virtual functions all parent states can override these functions
    public virtual void HandleInput(PlayerCharacter Controller, StateMachine machine) { }
    public virtual void HandleUpdate(PlayerCharacter Controller, StateMachine machine) { }
    public virtual void HandleFixedUpdate(PlayerCharacter Controller) { }
    public virtual void Enter(PlayerCharacter Controller) { }
    public virtual void Exit(PlayerCharacter Controller) { }



    //handle our possible transitional states, look up table by string for easy reading
    [SerializeField]
    protected State[] transitionAbleStates;

    protected Hashtable nextStates;

    //converst our list into a hashtable for easy look up later
    void BuildHashTable()
    {
        nextStates = new Hashtable();

        foreach (State dic in transitionAbleStates)
        {
            if (nextStates.ContainsKey(dic.GetName()))
                continue;

            nextStates.Add(dic.GetName(), dic);
        }
    }
}

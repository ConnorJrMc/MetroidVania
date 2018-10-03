using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : ScriptableObject
{
    public virtual void HandleInput(PlayerCharacter Controller) { }
    public virtual void HandleUpdate(PlayerCharacter Controller) { }
    public virtual void HandleFixedUpdate(PlayerCharacter Controller) { }
    public virtual void Enter(PlayerCharacter Controller) { }
    public virtual void Exit(PlayerCharacter Controller) { }
    [SerializeField]
    List<State> transitionableStates;

    [SerializeField]
    public int PriorityLevel = 0;
}

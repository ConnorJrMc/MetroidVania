using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "States/AirState")]
public class InAirState : State
{
    public override void HandleInput(PlayerCharacter Controller, StateMachine machine)
    {

    }


    public override void HandleUpdate(PlayerCharacter Controller, StateMachine machine)
    {
       if(Controller.CheckForGrounded())
       {
            machine.PushState((State)(nextStates["GroundedState"]), Controller);
       }

       Controller.AirborneHorizontalMovement();
       Controller.AirborneVerticalMovement();
    }
    public override void HandleFixedUpdate(PlayerCharacter Controller)
    {
        Controller.Move();
    }
    public override void Enter(PlayerCharacter Controller)
    {

    }
    public override void Exit(PlayerCharacter Controller)
    {

    }
}

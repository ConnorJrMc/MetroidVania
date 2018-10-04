using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//handle player grounded state
/*
 * Jump
 * Roll
 * dash
 */


[CreateAssetMenu(fileName = "States/GroundedState")]
public class GroundedState : State {
    public override void HandleInput(PlayerCharacter Controller, StateMachine machine)
    {
        //the different input that you can handle while in the grounded state
        //handle jump logic
        if (Controller.CheckForJumpInput())
        {
            if(machine.PushState((State)(nextStates["InAirState"]), Controller))
                Controller.SetVerticalMovement(Controller.jumpSpeed);
        }
    }
    public override void HandleUpdate(PlayerCharacter Controller,StateMachine machine)
    {
        Controller.GroundedHorizontalMovement(true);

        if(!Controller.CheckForGrounded())
        {
            machine.PushState((State)(nextStates["InAirState"]), Controller);
        }
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

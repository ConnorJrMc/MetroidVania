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
    public override void HandleInput(PlayerCharacter Controller)
    {
        //the different input that you can handle while in the grounded state
        //move left and right
        Controller.GroundedHorizontalMovement(true);
        Controller.CheckForGrounded();
        if(Controller.CheckForJumpInput())
            Controller.SetVerticalMovement(Controller.jumpSpeed);
    }
    public override void HandleUpdate(PlayerCharacter Controller)
    {

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

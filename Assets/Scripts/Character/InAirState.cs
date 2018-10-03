using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "States/AirState")]
public class InAirState : State
{
    public override void HandleInput(PlayerCharacter Controller)
    {
        //the different input that you can handle while in the grounded state
        //move left and right
        Controller.AirborneHorizontalMovement();
        Controller.AirborneVerticalMovement();
        Controller.CheckForGrounded();
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

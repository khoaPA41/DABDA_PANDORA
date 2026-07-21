using Script.StateMachine.Player.Base;
using UnityEngine;

public class HoldWallState : PlayerBaseState
{
    private static readonly int _holdWallAnimation = Animator.StringToHash("HoldWall");

    public HoldWallState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        playerStateMachine.JumpCount = 0;
        playerStateMachine.InputReader.JumpAction += playerStateMachine.SwitchJumpState;
        playerStateMachine.Animator.CrossFadeInFixedTime(_holdWallAnimation, playerStateMachine.AnimationCrossFade, 0);
    }

    public override void Tick(float deltaTime)
    {
        if (playerStateMachine.ForceReceiver.IsGrounded)
        {
            playerStateMachine.ReturnLocomotion();
        }

        if (!playerStateMachine.ForceReceiver.IsHoldWall)
        {
            Move(deltaTime);
        }
    }

    public override void Exit()
    {
        playerStateMachine.InputReader.JumpAction -= playerStateMachine.SwitchJumpState;
        playerStateMachine.ForceReceiver.IsHoldWall = false;
        playerStateMachine.ForceReceiver.IsSlideWall = false;
        playerStateMachine.ForceReceiver._verticalVelocity = 0f;
        playerStateMachine.transform.SetParent(null);
    }
}

using Script.StateMachine.Player.Base;
using UnityEngine;

public class SplineCartState : PlayerBaseState
{
    private readonly int _slideAnimation = Animator.StringToHash("Slide");
    private readonly int _runAnimation = Animator.StringToHash("Running");

    public SplineCartState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }
    

    public override void Enter()
    {
        playerStateMachine.TriggerChangeCameraAndInput.ChangeSplineCamera(true);
        playerStateMachine.JumpCount = 0;
        playerStateMachine.InputReader.JumpAction += playerStateMachine.SwitchJumpState;
        playerStateMachine.LastCartPosition = playerStateMachine.SplineCart.position;
        
        playerStateMachine.Animator.CrossFadeInFixedTime(playerStateMachine.IsRunningOnSpline ? _runAnimation : _slideAnimation,
            playerStateMachine.AnimationCrossFade, 0);
        // playerStateMachine.Animator.CrossFadeInFixedTime(_runAnimation,
        //     playerStateMachine.AnimationCrossFade, 0);
    }

    public override void Tick(float deltaTime)
    {
        MoveFollowSplineCart(deltaTime);
    }

    public override void Exit()
    {
        playerStateMachine.InputReader.JumpAction -= playerStateMachine.SwitchJumpState;

    }
}

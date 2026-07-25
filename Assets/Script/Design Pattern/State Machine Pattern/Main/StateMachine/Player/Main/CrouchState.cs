using Script.StateMachine.Player.Base;
using UnityEngine;

public class CrouchState : PlayerBaseState
{
    private static readonly int _movement = Animator.StringToHash("Movement");
    private readonly int _crouchBlendTreeHash = Animator.StringToHash("CrouchBlendTree");

    private float currentSpeed;
    public CrouchState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        playerStateMachine.JumpCount = 0;
        playerStateMachine.InputReader.JumpAction += playerStateMachine.SwitchJumpState;
        // playerStateMachine.InputReader.DashAction += playerStateMachine.SwitchDashState;
        playerStateMachine.InputReader.CrouchAction += playerStateMachine.ReturnLocomotion;
        playerStateMachine.ForceReceiver.FallingEventAction += playerStateMachine.SwitchInAirState;
        playerStateMachine.Interaction.PickUpItemAction += playerStateMachine.SwitchGetItemState;
        playerStateMachine.Interaction.EnterKeyAction += playerStateMachine.SwitchEnterKeyState;

        playerStateMachine.Animator.CrossFadeInFixedTime(_crouchBlendTreeHash, playerStateMachine.AnimationCrossFade, 0);
        playerStateMachine.IsMove = false;
        playerStateMachine.IsCrouch = true;
    }

    public override void Tick(float deltaTime)
    {
        Movement(deltaTime);
        UpdateAnimation(deltaTime);
    }
    
    private void Movement(float deltaTime)
    {
        currentSpeed = playerStateMachine.InputReader.IsSprint
            ? playerStateMachine.SprintSpeed
            : playerStateMachine.WalkSpeed;

        var motion = CalculateInputDirection();
        Move(motion * currentSpeed, deltaTime);
        FaceDir(motion, deltaTime);
    }

    private void UpdateAnimation(float deltaTime)
    {
        if (playerStateMachine.InputReader.Movement == Vector2.zero)
        {
            playerStateMachine.Animator.SetFloat(_movement, 0, playerStateMachine.AnimationCrossFade, deltaTime);
            if (playerStateMachine.Animator.GetFloat(_movement) <= 0.0001f)
            {
                playerStateMachine.Animator.SetFloat(_movement, 0);
            }

            return;
        }

        if (playerStateMachine.InputReader.IsSprint)
        {
            playerStateMachine.Animator.SetFloat(_movement, 1, playerStateMachine.AnimationCrossFade, deltaTime);
            return;
        }
            
        playerStateMachine.Animator.SetFloat(_movement, 0.5f, playerStateMachine.AnimationCrossFade, deltaTime);
    }

    public override void Exit()
    {
        playerStateMachine.InputReader.JumpAction -= playerStateMachine.SwitchJumpState;
        // playerStateMachine.InputReader.DashAction -= playerStateMachine.SwitchDashState;
        playerStateMachine.InputReader.CrouchAction -= playerStateMachine.ReturnLocomotion;
        playerStateMachine.ForceReceiver.FallingEventAction -= playerStateMachine.SwitchInAirState;
        playerStateMachine.Interaction.PickUpItemAction -= playerStateMachine.SwitchGetItemState;
        playerStateMachine.Interaction.EnterKeyAction -= playerStateMachine.SwitchEnterKeyState;
    }
}

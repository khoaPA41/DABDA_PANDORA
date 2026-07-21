using Script.StateMachine.Player.Base;
using UnityEngine;

namespace Script.StateMachine.Player.Main
{
    public class ClimbState : PlayerBaseState
    {
        private static readonly int _climbX = Animator.StringToHash("ClimbX");
        private static readonly int _climbY = Animator.StringToHash("ClimbY");

        private readonly int _climbingBlendTreeHash = Animator.StringToHash("ClimbingBlendTree");
        private float currentSpeed;

        public ClimbState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }

        public override void Enter()
        {
            playerStateMachine.InputReader.JumpAction += playerStateMachine.SwitchJumpState;
            playerStateMachine.Animator.CrossFadeInFixedTime(_climbingBlendTreeHash,
                playerStateMachine.AnimationCrossFade, 0);
        }

        public override void Tick(float deltaTime)
        {
            Movement(deltaTime);
            UpdateAnimation(deltaTime);
        }

        public override void Exit()
        {
            playerStateMachine.InputReader.JumpAction -= playerStateMachine.SwitchJumpState;
            playerStateMachine.ForceReceiver.IsClimbing = false;
        }

        private void Movement(float deltaTime)
        {
            currentSpeed = playerStateMachine.InputReader.IsSprint
                ? playerStateMachine.SprintSpeed
                : playerStateMachine.WalkSpeed;

            var motion = CalculateClimbInputDirection();
            Move(motion * currentSpeed, deltaTime);
        }

        private void UpdateAnimation(float deltaTime)
        {
            if (playerStateMachine.InputReader.Movement == Vector2.zero)
            {
                playerStateMachine.Animator.SetFloat(_climbX, 0, playerStateMachine.AnimationCrossFade, deltaTime);
                playerStateMachine.Animator.SetFloat(_climbY, 0, playerStateMachine.AnimationCrossFade, deltaTime);

                if (playerStateMachine.Animator.GetFloat(_climbX) <= 0.0001f)
                {
                    playerStateMachine.Animator.SetFloat(_climbX, 0);
                }
                
                
                if (playerStateMachine.Animator.GetFloat(_climbY) <= 0.0001f)
                {
                    playerStateMachine.Animator.SetFloat(_climbY, 0);
                }
        
                return;
            }
        
            // if (playerStateMachine.InputReader.IsSprint)
            // {
            //     playerStateMachine.Animator.SetFloat(_movement, 1, playerStateMachine.AnimationCrossFade, deltaTime);
            //     return;
            // }
            //
            playerStateMachine.Animator.SetFloat(_climbX, playerStateMachine.InputReader.Movement.x, playerStateMachine.AnimationCrossFade, deltaTime);
            playerStateMachine.Animator.SetFloat(_climbY, playerStateMachine.InputReader.Movement.y, playerStateMachine.AnimationCrossFade, deltaTime);
        }
    }
}
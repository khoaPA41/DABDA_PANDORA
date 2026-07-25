using Script.StateMachine.Player.Base;
using UnityEngine;

namespace Script.StateMachine.Player.Main
{
    public class InAirState : PlayerBaseState
    {
        private static readonly int _inAirAnimation = Animator.StringToHash("InAir");
        private float _previousTime;
        private float currentSpeed;

        private Vector3 matchTarget;
        public InAirState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }

        public override void Enter()
        {
            playerStateMachine.InputReader.JumpAction += BackToJump;
            playerStateMachine.InputReader.DashAction += SwitchDashState;
            // playerStateMachine.Interaction.ClimbAction += SwitchHoldWallState;
            playerStateMachine.ForceReceiver.OnMatchTargetAction += GetTarget;
            playerStateMachine.Animator.CrossFadeInFixedTime(_inAirAnimation, playerStateMachine.AnimationCrossFade, 0);
        }

        public override void Tick(float deltaTime)
        {
            if (playerStateMachine.ForceReceiver.IsGrounded)
            {
                if (playerStateMachine.IsOnSplineCart)
                {
                    playerStateMachine.SwitchState(new SplineCartState(playerStateMachine));
                    return;
                }

                playerStateMachine.SwitchState(new LandingState(playerStateMachine));
            }
            
            if (playerStateMachine.IsOnSplineCart)
            {
                MoveFollowSplineCart(deltaTime);
            }
            else
            {
                if (playerStateMachine.ForceReceiver.IsMatchTarget)
                {
                    MoveToTarget(matchTarget, deltaTime);
                }
                else
                {
                    Movement(deltaTime);
                }
            }
        }

        public override void Exit()
        {
            playerStateMachine.InputReader.DashAction -= SwitchDashState;
            playerStateMachine.InputReader.JumpAction -= BackToJump;
            // playerStateMachine.Interaction.ClimbAction -= SwitchHoldWallState;
            playerStateMachine.ForceReceiver.OnMatchTargetAction -= GetTarget;
            playerStateMachine.ForceReceiver.IsActiveFallingAction  = false;
            playerStateMachine.ForceReceiver.IsMatchTarget = false;
        }
        
        private void Movement(float deltaTime)
        {
            currentSpeed = playerStateMachine.InputReader.IsSprint
                ? playerStateMachine.SprintSpeed
                : playerStateMachine.WalkSpeed;

            var motion = CalculateInputDirection() * currentSpeed;
            Move(motion, deltaTime);
        }

        private void BackToJump()
        {
            if (playerStateMachine.JumpCount >= 2) return;
            playerStateMachine.ForceReceiver._verticalVelocity = 0f;
            playerStateMachine.SwitchState(new StartJumpState(playerStateMachine));
        }
        
        private void SwitchDashState()
        {
            playerStateMachine.SwitchState(new DashState(playerStateMachine));
        }
        
        private void SwitchHoldWallState()
        {
            playerStateMachine.SwitchState(new HoldWallState(playerStateMachine));
        }

        private void GetTarget(Vector3 position)
        {
            matchTarget =  position;
        }
    }
}

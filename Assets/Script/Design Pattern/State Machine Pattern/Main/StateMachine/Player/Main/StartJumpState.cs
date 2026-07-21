using System.Collections;
using Script.StateMachine.Player.Base;
using UnityEngine;

namespace Script.StateMachine.Player.Main
{
    public class StartJumpState : PlayerBaseState
    {
        private static readonly int _jumpAnimation = Animator.StringToHash("Jump");
        private static readonly string _jumpAnimationTag = "Jump";

        private float _previousTime;
        private float currentSpeed;
        private bool isIncreasingForce;
        private Vector3 dir;
        public StartJumpState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }

        public override void Enter()
        {
            playerStateMachine.ForceReceiver._verticalVelocity = -2f;
            playerStateMachine.JumpCount++;
            playerStateMachine.Animator.CrossFadeInFixedTime(_jumpAnimation, playerStateMachine.AnimationCrossFade, 0);
            playerStateMachine.ForceReceiver.Jump(playerStateMachine.JumpForce);
            FaceDir();
        }

        public override void Tick(float deltaTime)
        {
            var normalizeTime = GetNormalizeTime(playerStateMachine.Animator, _jumpAnimationTag, 0);
            if (normalizeTime >= _previousTime && normalizeTime >= .2f && !playerStateMachine.ForceReceiver.IsGrounded)
            {
                playerStateMachine.SwitchState(new InAirState(playerStateMachine));
            }
            
            _previousTime = normalizeTime;
            if (playerStateMachine.IsOnSplineCart)
            {
                MoveFollowSplineCart(deltaTime);
            }
            else
            {
                Movement(deltaTime);
            }
        }

        public override void Exit()
        {
            playerStateMachine.ForceReceiver.FallingEventAction -= playerStateMachine.SwitchInAirState;
        }
        
        private void Movement(float deltaTime)
        {
            currentSpeed = playerStateMachine.InputReader.IsSprint
                ? playerStateMachine.SprintSpeed
                : playerStateMachine.WalkSpeed;

            var motion = CalculateInputDirection() * currentSpeed;
            Move(motion, deltaTime);
        }

        private void FaceDir()
        {
            playerStateMachine.gameObject.transform.rotation = Quaternion.LookRotation(CalculateInputDirection());
        }
    }
}

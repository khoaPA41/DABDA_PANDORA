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
            playerStateMachine.Animator.CrossFadeInFixedTime(_climbingBlendTreeHash,
                playerStateMachine.AnimationCrossFade, 0);
        }

        public override void Tick(float deltaTime)
        {
            Movement(deltaTime);
            // UpdateAnimation(deltaTime);
        }

        public override void Exit()
        {
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

        //     private void UpdateAnimation(float deltaTime)
        //     {
        //         if (playerStateMachine.InputReader.Movement == Vector2.zero)
        //         {
        //             playerStateMachine.Animator.SetFloat(_movement, 0, playerStateMachine.AnimationCrossFade, deltaTime);
        //             if (playerStateMachine.Animator.GetFloat(_movement) <= 0.0001f)
        //             {
        //                 playerStateMachine.Animator.SetFloat(_movement, 0);
        //             }
        //
        //             return;
        //         }
        //
        //         if (playerStateMachine.InputReader.IsSprint)
        //         {
        //             playerStateMachine.Animator.SetFloat(_movement, 1, playerStateMachine.AnimationCrossFade, deltaTime);
        //             return;
        //         }
        //         
        //         playerStateMachine.Animator.SetFloat(_movement, 0.5f, playerStateMachine.AnimationCrossFade, deltaTime);
        //     }
        
    }
}
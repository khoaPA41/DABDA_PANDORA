using Script.StateMachine.Player.Base;
using Script.StateMachine.Player.Main;
using UnityEngine;

namespace Script.StateMachine.Player.Main
{
    public class DashState : PlayerBaseState
    {
        private static readonly int _dashAnimation = Animator.StringToHash("Dash");
        private static readonly string _dashAnimationTag = "Dash";

        private float _previousTime;

        public DashState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }

        public override void Enter()
        {
            // playerStateMachine.Interaction.ClimbAction += SwitchHoldWallState;
            playerStateMachine.Animator.CrossFadeInFixedTime(_dashAnimation, playerStateMachine.AnimationCrossFade, 0);
            playerStateMachine.ForceReceiver.AddImpact(new Vector3(0f, 0f, CalculateInputDirection().z) *
                                                       playerStateMachine.DashForce);
        }

        public override void Tick(float deltaTime)
        {
            var normalizeTime = GetNormalizeTime(playerStateMachine.Animator, _dashAnimationTag, 0);

            if (normalizeTime >= _previousTime && normalizeTime >= .5f)
            {
                if (!playerStateMachine.CharacterController.isGrounded)
                {
                    playerStateMachine.SwitchState(new InAirState(playerStateMachine));
                    return;
                }
                playerStateMachine.ReturnLocomotion();
            }

            _previousTime = normalizeTime;
            Move(deltaTime);
        }

        public override void Exit()
        {
            playerStateMachine.ForceReceiver.IsDash = false;
            // playerStateMachine.Interaction.ClimbAction -= SwitchHoldWallState;
            
        }
        

    }
}

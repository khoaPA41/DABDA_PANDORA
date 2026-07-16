using Script.StateMachine.Player.Base;
using UnityEngine;

namespace Script.StateMachine.Player.Main
{
    public class LandingState : PlayerBaseState
    {
        private static readonly int _landingAnimation = Animator.StringToHash("Landing");
        private static readonly string _landingAnimationTag = "Landing";
        private float _previousTime;

        public LandingState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }

        public override void Enter()
        {
            
            playerStateMachine.Animator.CrossFadeInFixedTime(_landingAnimation, playerStateMachine.AnimationCrossFade,
                0);
        }

        public override void Tick(float deltaTime)
        {
            var normalizeTime = GetNormalizeTime(playerStateMachine.Animator, _landingAnimationTag, 0);

            if (normalizeTime >= _previousTime && normalizeTime >= .2f)
            {
                playerStateMachine.ReturnLocomotion();
            }

            _previousTime = normalizeTime;
        }

        public override void Exit()
        {
        }
    }
}

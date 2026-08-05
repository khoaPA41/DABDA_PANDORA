using Script.StateMachine.Player.Base;
using UnityEngine;

namespace Script.StateMachine.Player.Main
{
    public class LocomotionState : PlayerBaseState
    {
        private static readonly int _movement = Animator.StringToHash("Movement");
        private readonly int _locomotionBlendTreeHash = Animator.StringToHash("LocomotionBlendTree");

        private float currentSpeed;
        
        public LocomotionState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }

        public override void Enter()
        {
            playerStateMachine.JumpCount = 0;
            playerStateMachine.InputReader.JumpAction += playerStateMachine.SwitchJumpState;
            playerStateMachine.InputReader.DashAction += playerStateMachine.SwitchDashState;
            playerStateMachine.InputReader.CrouchAction += SwitchCrouchState;
            playerStateMachine.ForceReceiver.FallingEventAction += playerStateMachine.SwitchInAirState;
            playerStateMachine.Interaction.PickUpItemAction += playerStateMachine.SwitchGetItemState;
            playerStateMachine.Interaction.EnterKeyAction += playerStateMachine.SwitchEnterKeyState;

            playerStateMachine.Animator.CrossFadeInFixedTime(_locomotionBlendTreeHash, playerStateMachine.AnimationCrossFade, 0);
        }

        public override void Tick(float deltaTime)
        {
            if (GameManager.Instance.isGetTheFinalKey)
            {
                Debug.Log(GameManager.Instance.isGetTheFinalKey);
                playerStateMachine.GetPooledObject.GetObject("Key_Name_IV", Vector3.zero, playerStateMachine.HoldItemTransform);
                playerStateMachine.Interaction.GetKeyName("Key_Name_IV");
                GameManager.Instance.isGetTheFinalKey = false;
                return;
            }
            
            if (playerStateMachine.InputReader.Movement == Vector2.zero)
            {
                playerStateMachine.IsMove = false;
            }
            else
            {
                playerStateMachine.IsMove = true;
            }
            
            Movement(deltaTime);
            UpdateAnimation(deltaTime);
        }

        public override void Exit()
        {
            playerStateMachine.InputReader.JumpAction -= playerStateMachine.SwitchJumpState;
            playerStateMachine.InputReader.DashAction -= playerStateMachine.SwitchDashState;
            playerStateMachine.InputReader.CrouchAction -= SwitchCrouchState;
            playerStateMachine.ForceReceiver.FallingEventAction -= playerStateMachine.SwitchInAirState;
            playerStateMachine.Interaction.PickUpItemAction -= playerStateMachine.SwitchGetItemState;
            playerStateMachine.Interaction.EnterKeyAction -= playerStateMachine.SwitchEnterKeyState;
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

        private void SwitchCrouchState()
        {
            playerStateMachine.SwitchState(new CrouchState(playerStateMachine));
        }

        // private void SwitchGetItemState(GameObject item)
        // {
        //     playerStateMachine.SwitchState(new GetItemState(playerStateMachine, item));
        // }
        //
        // private void SwitchEnterKeyState()
        // {
        //     playerStateMachine.SwitchState(new EnterKeyState(playerStateMachine));
        // }
    }
}
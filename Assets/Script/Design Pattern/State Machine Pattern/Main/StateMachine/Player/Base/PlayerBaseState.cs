using Script.StateMachine.Base;
using UnityEngine;

namespace Script.StateMachine.Player.Base
{
    public abstract class PlayerBaseState : State
    {
        protected readonly PlayerStateMachine playerStateMachine;
        
        protected PlayerBaseState(PlayerStateMachine playerStateMachine)
        {
            this.playerStateMachine = playerStateMachine;
        }

        protected void Move(Vector3 motion, float deltaTime)
        {
            playerStateMachine.CharacterController.Move((playerStateMachine.ForceReceiver.Movement + motion) * deltaTime);
        }
        
        protected void Move(float deltaTime)
        {
            playerStateMachine.CharacterController.Move((playerStateMachine.ForceReceiver.Movement + Vector3.zero) * deltaTime);
        }
        
        protected void FaceDir(Vector3 movement, float deltaTime)
        {
            if (movement == Vector3.zero)
            {
                return;
            }

            var rotationDamping = playerStateMachine.TriggerChangeCameraAndInput.Is3DState
                ? playerStateMachine.RotationDamping
                : playerStateMachine.RotationDamping2D;

            playerStateMachine.transform.rotation = Quaternion.Lerp(playerStateMachine.transform.rotation,
                Quaternion.LookRotation(movement), rotationDamping * deltaTime);
        }

        protected void FaceDirJump(Vector3 movement)
        {
            if (movement == Vector3.zero)
            {
                return;
            }

            playerStateMachine.transform.rotation = Quaternion.LookRotation(movement);
        }

        protected Vector3 CalculateInputDirection()
        {
            if (playerStateMachine.TriggerChangeCameraAndInput.Is3DState)
            {
                var forward = playerStateMachine.MainCameraTransform.transform.forward;
                var right = playerStateMachine.MainCameraTransform.transform.right;
                forward.y = 0f;
                right.y = 0f;
                forward.Normalize();
                right.Normalize();
                return forward * playerStateMachine.InputReader.Movement.y + right * playerStateMachine.InputReader.Movement.x;
            }
            return new Vector3(-playerStateMachine.InputReader.Movement.y, 0, playerStateMachine.InputReader.Movement.x);
        }
    }
}
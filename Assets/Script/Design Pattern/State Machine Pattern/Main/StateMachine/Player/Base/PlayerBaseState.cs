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

        protected void MoveFollowSplineCart(float deltaTime)
        {
            var cartDelta = Vector3.zero;
            
            // Calculate delta S of Spline Cart
            cartDelta = playerStateMachine.SplineCart.position - playerStateMachine.LastCartPosition;
            playerStateMachine.LastCartPosition =  playerStateMachine.SplineCart.position;
            
            // Gravity of horizontal
            var horizontalSpline = playerStateMachine.SplineCart.right * (playerStateMachine.InputReader.Movement.x * playerStateMachine.SprintSpeed * deltaTime);
            
            // Gravity of falling / jumping
            var playerDelta = playerStateMachine.ForceReceiver.Movement * deltaTime;

            // Face look at forward
            FaceDir(playerStateMachine.MainCameraTransform.transform.forward, deltaTime);
            
            // Combine forces
            playerStateMachine.CharacterController.Move(playerDelta + cartDelta + horizontalSpline);
            
            //Convert Player World Space to Spline Local
            var localPosToCart =  playerStateMachine.SplineCart.InverseTransformPoint(playerStateMachine.transform.position);
            localPosToCart.x = Mathf.Clamp(localPosToCart.x, -5f, 5f);
            // Attach player local pos to player world pos
            playerStateMachine.transform.position = playerStateMachine.SplineCart.TransformPoint(localPosToCart);

        }
        
        protected void FaceDir(Vector3 movement, float deltaTime)
        {
            if (movement == Vector3.zero)
            {
                return;
            }

            var rotationDamping = playerStateMachine.TriggerChangeCameraAndInput.IsChangeInputState
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
            if (playerStateMachine.TriggerChangeCameraAndInput.IsChangeInputState)
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
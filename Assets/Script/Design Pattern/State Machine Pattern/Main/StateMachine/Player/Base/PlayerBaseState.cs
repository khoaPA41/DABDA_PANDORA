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
            var horizontalInput = playerStateMachine.IsRunningOnSpline
                ? -playerStateMachine.InputReader.Movement.x
                : playerStateMachine.InputReader.Movement.x;
            var horizontalSpline = playerStateMachine.SplineCart.right * (horizontalInput * playerStateMachine.SprintSpeed * deltaTime);
            
            // Gravity of falling / jumping
            var playerDelta = playerStateMachine.ForceReceiver.Movement * deltaTime;

            // Face look at forward
            var forwardDir = playerStateMachine.IsRunningOnSpline
                ? -playerStateMachine.MainCameraTransform.transform.forward
                : playerStateMachine.MainCameraTransform.transform.forward;
            
            FaceDir(forwardDir, deltaTime);
            
            // Combine forces
            playerStateMachine.CharacterController.Move(playerDelta + cartDelta + horizontalSpline);
            
            //Convert Player World Space to Spline Local
            var localPosToCart =  playerStateMachine.SplineCart.InverseTransformPoint(playerStateMachine.transform.position);
            localPosToCart.x = Mathf.Clamp(localPosToCart.x, -5f, 5f);
            localPosToCart.z = 0f;

            // Attach player local pos to player world pos
            playerStateMachine.transform.position = playerStateMachine.SplineCart.TransformPoint(localPosToCart);
        }

        protected void MoveToTarget(Vector3 movement, float deltaTime)
        {
            var distance= movement - playerStateMachine.transform.position;
            // var lerpPosition = Vector3.MoveTowards(playerStateMachine.transform.position, movement, playerStateMachine.SprintSpeed * 3 * deltaTime); 
            // var playerDelta = playerStateMachine.ForceReceiver.Movement * deltaTime;
            // var movementDelta = lerpPosition - playerStateMachine.transform.position;
            // playerStateMachine.CharacterController.Move(movementDelta);
            playerStateMachine.CharacterController.Move(distance);

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
            return new Vector3(-playerStateMachine.InputReader.Movement.y, 0f, playerStateMachine.InputReader.Movement.x);
        }

        protected Vector3 CalculateClimbInputDirection()
        {
            var right = playerStateMachine.MainCameraTransform.transform.right;
            right.y = 0f;
            right.Normalize();


            var climbSurfaceNormal = playerStateMachine.ForceReceiver.SurfaceNormal;
            var climbUp = Vector3.ProjectOnPlane(Vector3.up, climbSurfaceNormal).normalized;
            var climbRight = Vector3.ProjectOnPlane(right, climbSurfaceNormal).normalized;
            
            return (climbUp * playerStateMachine.InputReader.Movement.y +
                   climbRight * playerStateMachine.InputReader.Movement.x);
            // - climbSurfaceNormal * .1f
        }

        protected void RotateToSurface(float deltaTime)
        {
            var climbSurfaceNormal = playerStateMachine.ForceReceiver.SurfaceNormal;
            if (climbSurfaceNormal != Vector3.zero)
            {
                playerStateMachine.transform.rotation = Quaternion.Lerp(playerStateMachine.transform.rotation, Quaternion.LookRotation(-climbSurfaceNormal), playerStateMachine.RotationDamping * deltaTime);
            }
        }
    }
}
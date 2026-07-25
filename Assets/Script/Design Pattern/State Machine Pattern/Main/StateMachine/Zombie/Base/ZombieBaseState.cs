using Script.StateMachine.Base;
using Script.StateMachine.Player.Base;
using UnityEngine;

namespace Script.StateMachine.Zombie.Base
{
    public abstract class ZombieBaseState : State
    {
        protected readonly ZombieStateMachine zombieStateMachine;

        protected ZombieBaseState(ZombieStateMachine zombieStateMachine)
        {
            this.zombieStateMachine = zombieStateMachine;
        }


        protected void MoveToTarget(Vector3 target, float deltaTime)
        {
            var direction = target - zombieStateMachine.transform.position;
            zombieStateMachine.CharacterController.Move(direction * zombieStateMachine.SprintSpeed * deltaTime);
        }

        protected void FaceDir(Vector3 target, float deltaTime)
        {
            var direction = target - zombieStateMachine.transform.position;
            zombieStateMachine.transform.rotation = Quaternion.Slerp(zombieStateMachine.transform.rotation,
                Quaternion.LookRotation(direction), 10f * deltaTime);
        }

        protected bool CheckAttackRange()
        {
            return (zombieStateMachine.Player.position - zombieStateMachine.transform.position).sqrMagnitude <=
                   zombieStateMachine.AttackRadius * zombieStateMachine.AttackRadius;
        }
        
        protected bool CheckChasingRange()
        {
            var directionToPlayer = zombieStateMachine.Player.position - zombieStateMachine.transform.position;
            var sqrtDistanceToPlayer = directionToPlayer.sqrMagnitude;

            if (sqrtDistanceToPlayer > zombieStateMachine.ChasingRadius * zombieStateMachine.ChasingRadius)
            {
                return false;
            }
            
            // Debug.Log("In Chasing Range");
            
            var player = zombieStateMachine.Player.GetComponent<PlayerStateMachine>();
            if (player.IsMove) return true;

            var angleToPlayer = Vector3.Angle(zombieStateMachine.transform.forward, directionToPlayer);

            // Debug.Log("Angle: " + angleToPlayer);
            if (angleToPlayer <= zombieStateMachine.ViewAngle / 2f)
            {
                // Debug.Log("See Player" +  zombieStateMachine.Player.name);
            
                var origin = zombieStateMachine.transform.position + Vector3.up;
                var target = zombieStateMachine.Player.position + Vector3.up;
            
                if (!Physics.Linecast(origin, target, zombieStateMachine.ObstacleLayer))
                {
                    // Debug.Log(zombieStateMachine.Player.name + " is chasing");
                    return true;
                }
            }

            return false;
        }
    }
}
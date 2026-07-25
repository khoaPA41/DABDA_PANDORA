using Script.StateMachine.Zombie.Base;
using UnityEngine;
using UnityEngine.AI;

public class ZombiePatrolState : ZombieBaseState
{
    private static readonly int _walkAnimation = Animator.StringToHash("Z_Walk");

    public ZombiePatrolState(ZombieStateMachine zombieStateMachine) : base(zombieStateMachine)
    {
    }

    public override void Enter()
    {
        zombieStateMachine.Animator.CrossFadeInFixedTime(_walkAnimation, zombieStateMachine.AnimationCrossFade);
        zombieStateMachine.NavMeshAgent.isStopped = false;
        SetDestination();
    }

    public override void Tick(float deltaTime)
    {
        if (CheckChasingRange())
        {
            zombieStateMachine.SwitchState(new ZombieChasingState(zombieStateMachine));
        }
        
        if (!zombieStateMachine.NavMeshAgent.pathPending && zombieStateMachine.NavMeshAgent.remainingDistance <=
            zombieStateMachine.NavMeshAgent.stoppingDistance)
        {
            zombieStateMachine.SwitchState(new ZombieLocomotionState(zombieStateMachine));
        }
    }

    public override void Exit()
    {
    }


    private void SetDestination()
    {
        if (TryGetRandomPoint(zombieStateMachine.transform.position, zombieStateMachine.PatrolRadius, out var randomPoint))
        {
            zombieStateMachine.NavMeshAgent.SetDestination(randomPoint);
        }
    }

    private bool TryGetRandomPoint(Vector3 center, float patrolRadius, out Vector3 result)
    {
        
        // Get the random point in sphere scope
        var randomPoint = Random.insideUnitSphere * patrolRadius;
        
        // Add random point with center (position of AI) to convert world space
        randomPoint += center;

        NavMeshHit hit;
        
        // Get the point nearest random point in sphere scope

        if (NavMesh.SamplePosition(randomPoint, out hit, patrolRadius, 1))
        {
            result = hit.position;
            return true;
        }
        
        result = Vector3.zero;
        return false;
    }
}

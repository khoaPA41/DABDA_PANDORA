using Script.StateMachine.Zombie.Base;
using UnityEngine;

public class ZombieChasingState : ZombieBaseState
{
    private static readonly int _runAnimation = Animator.StringToHash("Z_Run");

    public ZombieChasingState(ZombieStateMachine zombieStateMachine) : base(zombieStateMachine)
    {
    }

    public override void Enter()
    {
        zombieStateMachine.Animator.CrossFadeInFixedTime(_runAnimation, zombieStateMachine.AnimationCrossFade);
        zombieStateMachine.NavMeshAgent.speed = zombieStateMachine.SprintSpeed;
    }

    public override void Tick(float deltaTime)
    {
        if (CheckAttackRange())
        {
            zombieStateMachine.SwitchState(new ZombieAttackState(zombieStateMachine));
        }
        
        MoveToTarget(zombieStateMachine.Player.position);
    }

    public override void Exit()
    {
    }
}

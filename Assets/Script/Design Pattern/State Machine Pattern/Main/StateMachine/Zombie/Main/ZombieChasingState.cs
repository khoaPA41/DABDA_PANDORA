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

    }

    public override void Tick(float deltaTime)
    {
        if (CheckAttackRange())
        {
            zombieStateMachine.SwitchState(new ZombieAttackState(zombieStateMachine));
        }
        MoveToTarget(zombieStateMachine.Player.position, deltaTime);
        FaceDir(zombieStateMachine.Player.position, deltaTime);
    }

    public override void Exit()
    {
    }
}

using Script.StateMachine.Zombie.Base;
using UnityEngine;

public class ZombieLocomotionState : ZombieBaseState
{
    private static readonly int _locomotionAnimation = Animator.StringToHash("Z_Idle");

    private float countTime = 0f;
    public ZombieLocomotionState(ZombieStateMachine zombieStateMachine) : base(zombieStateMachine)
    {
    }

    public override void Enter()
    {
        zombieStateMachine.Animator.CrossFadeInFixedTime(_locomotionAnimation, zombieStateMachine.AnimationCrossFade);
    }

    public override void Tick(float deltaTime)
    {
        if (CheckChasingRange())
        {
            zombieStateMachine.SwitchState(new ZombieChasingState(zombieStateMachine));
        }
        
        countTime += deltaTime;
        if (countTime >= 1f)
        {
            zombieStateMachine.SwitchState(new ZombiePatrolState(zombieStateMachine));
        }
    }

    public override void Exit()
    {
    }
}

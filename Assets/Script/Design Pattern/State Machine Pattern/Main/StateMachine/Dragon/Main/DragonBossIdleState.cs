using Script.StateMachine.Player.Base;
using UnityEngine;

public class DragonBossIdleState : DragonBossBaseState
{
    private readonly int IdleAnimationHash = Animator.StringToHash("Dragon_Idle");
    public DragonBossIdleState(DragonBossStateMachine dragonStateMachine) : base(dragonStateMachine)
    {
    }

    public override void Enter()
    {
        dragonStateMachine.Animator.CrossFadeInFixedTime(IdleAnimationHash, dragonStateMachine.AnimationCrossFade);
    }

    public override void Tick(float deltaTime)
    {
        MoveToTarget(deltaTime);
    }

    public override void Exit()
    {
        
    }
}

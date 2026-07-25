using Script.StateMachine.Player.Base;
using Script.StateMachine.Zombie.Base;
using UnityEngine;

public class ZombieAttackState : ZombieBaseState
{
    private static readonly int _attackAnimation = Animator.StringToHash("Z_Attack");
    private static readonly string _attackAnimationString = "Z_Attack";

    private float previousTime;
    public ZombieAttackState(ZombieStateMachine zombieStateMachine) : base(zombieStateMachine)
    {
    }

    public override void Enter()
    {
        zombieStateMachine.Animator.CrossFadeInFixedTime(_attackAnimation, zombieStateMachine.AnimationCrossFade);

    }

    public override void Tick(float deltaTime)
    {
        var normalizeTime = GetNormalizeTime(zombieStateMachine.Animator, _attackAnimationString, 0);


        if (normalizeTime > previousTime && normalizeTime <= .9f)
        {
            zombieStateMachine.Player.SetParent(zombieStateMachine.Hand);
            zombieStateMachine.Player.GetComponent<PlayerStateMachine>().CallDeathAction();
        }
        
        previousTime = normalizeTime;
        FaceDir(zombieStateMachine.Player.position, deltaTime);
    }

    public override void Exit()
    {
    }
}

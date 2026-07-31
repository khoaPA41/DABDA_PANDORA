using Script.StateMachine.Player.Base;
using UnityEngine;

public class DragonBossAttackState : DragonBossBaseState
{
    private readonly int AttackRightAnimationHash = Animator.StringToHash("Attack_Right");
    private readonly int AttackLeftAnimationHash = Animator.StringToHash("Attack_Left");

    private bool isRight;
    private float countTime;
    public DragonBossAttackState(DragonBossStateMachine dragonStateMachine, bool isRight) : base(dragonStateMachine)
    {
        this.isRight = isRight;
    }

    public override void Enter()
    {
        countTime = Time.time + dragonStateMachine.timeToChangeAttack;
        var index = Random.Range(0, dragonStateMachine.BulletName.Count);
        dragonStateMachine.Animator.CrossFadeInFixedTime(isRight ? AttackRightAnimationHash : AttackLeftAnimationHash, dragonStateMachine.AnimationCrossFade);
        dragonStateMachine.StartCoroutine(WaitToNextShoot(index, dragonStateMachine.BulletTime[index]));
    }

    public override void Tick(float deltaTime)
    {
        if (Time.time >= countTime)
        {
            dragonStateMachine.SwitchState(new DragonBossAttackState(dragonStateMachine, !isRight));
        }
    }

    public override void Exit()
    {
    }
}

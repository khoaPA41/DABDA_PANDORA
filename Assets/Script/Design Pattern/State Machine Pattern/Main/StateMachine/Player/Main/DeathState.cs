using Script.StateMachine.Player.Base;
using UnityEngine;

public class DeathState : PlayerBaseState
{
    private static readonly int _deathAnimation = Animator.StringToHash("Death");
    private static readonly string _deathAnimationTag = "Death";
    private float _previousTime;
    public DeathState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        playerStateMachine.Animator.CrossFadeInFixedTime(_deathAnimation, playerStateMachine.AnimationCrossFade, 0);
    }

    public override void Tick(float deltaTime)
    {
        var normalizeTime = GetNormalizeTime(playerStateMachine.Animator, _deathAnimationTag, 0);

        if (normalizeTime >= _previousTime && normalizeTime >= .5f)
        {
            
        }

        _previousTime = normalizeTime;
    }

    public override void Exit()
    {
    }
}

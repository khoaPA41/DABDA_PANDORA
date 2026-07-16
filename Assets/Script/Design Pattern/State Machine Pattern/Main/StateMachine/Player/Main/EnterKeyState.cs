using Script.StateMachine.Player.Base;
using UnityEngine;

public class EnterKeyState : PlayerBaseState
{    
    private static readonly int _enterKeyAnimation = Animator.StringToHash("EnterKey");
    private static readonly string _enterKeyAnimationTag = "EnterKey";
    private float _previousTime;
    
    public EnterKeyState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        playerStateMachine.Animator.CrossFadeInFixedTime(_enterKeyAnimation, playerStateMachine.AnimationCrossFade, 0);
    }

    public override void Tick(float deltaTime)
    {
        var normalizeTime = GetNormalizeTime(playerStateMachine.Animator, _enterKeyAnimationTag, 0);
        if (normalizeTime >= _previousTime && normalizeTime >= .8f)
        {
            playerStateMachine.InteractionHoldWall.ActiveText(playerStateMachine.GetPooledObject.item.name);
            playerStateMachine.GetPooledObject.item.Release(playerStateMachine.GetPooledObject.item.name);
            playerStateMachine.TriggerChangeCameraAndInput.ChangeCameraTargetGateCoroutine();
            playerStateMachine.ReturnLocomotion();
            return;
        }

        _previousTime = normalizeTime;
    }

    public override void Exit()
    {
        // playerStateMachine.TriggerChangeCameraAndInput.ChangeCameraState();
    }
}

using Script.StateMachine.Player.Base;
using UnityEngine;
using UnityEngine.Rendering;

public class GetItemState : PlayerBaseState
{
    private static readonly int _getItemAnimation = Animator.StringToHash("GetItem");
    private static readonly string _getItemAnimationTag = "GetItem";

    private GameObject item;
    private float _previousTime;
    public GetItemState(PlayerStateMachine playerStateMachine, GameObject item) : base(playerStateMachine)
    {
        this.item = item;
    }

    public override void Enter()
    {
        playerStateMachine.Animator.CrossFadeInFixedTime(_getItemAnimation, playerStateMachine.AnimationCrossFade, 0);

    }

    public override void Tick(float deltaTime)
    {
        var normalizeTime = GetNormalizeTime(playerStateMachine.Animator, _getItemAnimationTag, 0);
        if (normalizeTime >= _previousTime && normalizeTime >= .5f)
        {
            playerStateMachine.DestroyObject(item);
            playerStateMachine.GetPooledObject.GetObject(item.name, Vector3.zero, playerStateMachine.HoldItemTransform);
            playerStateMachine.ReturnLocomotion();
            return;
        }

        _previousTime = normalizeTime;
    }

    public override void Exit()
    {
    }
}

using Script.StateMachine.Player.Main;
using UnityEngine;

namespace Script.StateMachine.Player.Base
{
    public class PlayerStateMachine : StateMachine.Base.StateMachine
    {
        [Header("Input")][field: SerializeField]
        public InputReader InputReader { get; private set; }
        
        [Header("Camera")][field: SerializeField]
        public TriggerChangeCameraAndInput TriggerChangeCameraAndInput { get; private set; }
        
        [Header("Physics")]
        [field: SerializeField]
        public CharacterController CharacterController { get; private set; }
        [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
        [field: SerializeField] public InteractionHoldWall InteractionHoldWall { get; private set; }
        [field: SerializeField] public float WalkSpeed { get; set; } = 5f;
        [field: SerializeField] public float SprintSpeed { get; set; } = 5f;
        [field: SerializeField] public float MovementSpeedStunnedCoefficient { get; private set; } = .2f;
        [field: SerializeField] public float RotationDamping { get; private set; } = .5f;
        [field: SerializeField] public float RotationDamping2D { get; private set; } = 10f;
        [field: SerializeField] public float JumpForce { get; private set; }
        [field: SerializeField] public float DashForce { get; private set; }

        [Header("Item")]         
        [field: SerializeField] public GetPooledObject GetPooledObject { get; private set; }
        [field: SerializeField] public Transform HoldItemTransform { get; private set; }

        
        [Header("Animation")]
        [field: SerializeField]
        public Animator Animator { get; private set; }

        [field: SerializeField] public float AnimationCrossFade { get; private set; } = .1f;

        public Transform MainCameraTransform { get; private set; }
        // public bool Is3dEnvironment {get; private set;} = false;
        
        public int JumpCount { get; set; }
        private void Start()
        {
            if (Camera.main is not null) MainCameraTransform = Camera.main.transform;
            ReturnLocomotion();
        }
        
        // private void OnEnable()
        // {
        //     ForceReceiver.FallingEventAction += SwitchInAirState;
        // }
        //
        // private void OnDisable()
        // {
        //     ForceReceiver.FallingEventAction -= SwitchInAirState;
        // }
        
        public void ReturnLocomotion()
        {
            SwitchState(new LocomotionState(this));
        }
        
        public void SwitchDashState()
        {
            SwitchState(new DashState(this));
        }
        
        public void SwitchJumpState()
        {
            ForceReceiver.IsHoldWall = false;
            SwitchState(new StartJumpState(this));
        }
        
        public void SwitchInAirState()
        {
            SwitchState(new InAirState(this));
        }

        public void DestroyObject(GameObject obj)
        {
            Destroy(obj);
        }
    }
}
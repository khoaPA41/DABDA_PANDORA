using UnityEngine;
using UnityEngine.AI;

namespace Script.StateMachine.Zombie.Base
{
    public class ZombieStateMachine : StateMachine.Base.StateMachine
    {
        [field: Header("Physics")]
        [field: SerializeField] public NavMeshAgent NavMeshAgent { get; set; }
        [field: SerializeField] public float SprintSpeed { get; set; } = 5f;
        [field: SerializeField] public Transform Hand { get; set; }

        [field: Header("Animation")]
        [field: SerializeField]
        public Animator Animator { get; private set; }
        [field: SerializeField] public float AnimationCrossFade { get; private set; } = .1f;

        [field: Header("Patrol Range")]
        [field: SerializeField] public float PatrolRadius { get; private set; }

        [field: Header("Chasing Range")]
        [field: SerializeField] public float ChasingRadius { get; private set; }

        [field: Header("Attack Range")]
        [field: SerializeField] public float AttackRadius { get; private set; }

        [field: Header("View Angle")]
        [field: SerializeField] public float ViewAngle { get; private set; }

        [field: Header("View Layer")]
        [field: SerializeField] public LayerMask ObstacleLayer { get; private set; }


        public Transform Player { get; private set; }
        public Transform Spline { get; private set; }
        public Transform LastCart { get; set; }

        private void Start()
        {
            Player = GameObject.FindWithTag("Player").transform;
            Spline = GameObject.FindWithTag("Spline").transform;
            ReturnLocomotion();
        }

        public void ReturnLocomotion()
        {
            SwitchState(new ZombieLocomotionState(this));
        }
    }
}
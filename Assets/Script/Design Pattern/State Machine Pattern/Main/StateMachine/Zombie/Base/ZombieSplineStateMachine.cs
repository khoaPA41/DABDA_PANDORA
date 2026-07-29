using UnityEngine;
using UnityEngine.AI;

namespace Script.StateMachine.Zombie.Base
{
    public class ZombieSplineStateMachine : StateMachine.Base.StateMachine
    {
        [Header("Physics")]
        [field: SerializeField]
        public CharacterController CharacterController { get; private set; }
        [field: SerializeField] public NavMeshAgent NavMeshAgent { get; set; }
        [field: SerializeField] public float SprintSpeed { get; set; } = 5f;
        [field: SerializeField] public Transform Hand { get; set; }

        [Header("Animation")]
        [field: SerializeField]
        public Animator Animator { get; private set; }
        [field: SerializeField] public float AnimationCrossFade { get; private set; } = .1f;

        [Header("Patrol Range")]
        [field: SerializeField] public float PatrolRadius { get; private set; }
        
        public Transform Player;
        
        
        private void Start()
        {
            // SwitchState(new ZombieChasingState(this));
        }
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Script.StateMachine.Player.Base
{
    public class DragonBossStateMachine : StateMachine.Base.StateMachine
    {
        [Header("Animation")]
        [field: SerializeField]
        public Animator Animator { get; private set; }
        [field: SerializeField] public float AnimationCrossFade { get; private set; } = .1f;
        
        [Header("Attack")]
        [field: SerializeField]
        public Shooting Shooting { get; private set; }
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public Transform MainProjectile { get; private set; }
        [field: SerializeField] public List<string> BulletName { get; private set; }
        [field: SerializeField] public List<float> BulletTime { get; private set; }

        [field: SerializeField] public float timeToChangeAttack { get; private set; }
        [field: SerializeField] public DragonHealth DragonHealth { get; private set; }
        [field: SerializeField] public PlayableDirector EndCutscene { get; private set; }

        public Camera MainCamera { get; private set; }
        
        private void Start()
        {
            MainCamera = Camera.main;
            SwitchState(new DragonBossIdleState(this));
        }

        private void OnEnable()
        {
            DragonHealth.OnDeath += ActiveEndCutscene;
        }

        private void OnDisable()
        {
            DragonHealth.OnDeath -= ActiveEndCutscene;
        }
        
        private void ActiveEndCutscene()
        {
            Debug.Log("end cutscene");
            EndCutscene.Play();
        }
    }
}

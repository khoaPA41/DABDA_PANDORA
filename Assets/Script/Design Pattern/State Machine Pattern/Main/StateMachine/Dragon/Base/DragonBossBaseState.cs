using System.Collections;
using Script.StateMachine.Base;
using UnityEngine;

namespace Script.StateMachine.Player.Base
{
    public abstract class DragonBossBaseState : State
    {
        protected DragonBossStateMachine dragonStateMachine;

        protected DragonBossBaseState(DragonBossStateMachine dragonStateMachine)
        {
            this.dragonStateMachine = dragonStateMachine;
        }


        protected void MoveToTarget(float deltaTime)
        {
            if (dragonStateMachine.MainCamera.WorldToViewportPoint(dragonStateMachine.transform.position).y <= .8f)
            {
                dragonStateMachine.SwitchState(new DragonBossAttackState(dragonStateMachine, true));
            }
            dragonStateMachine.transform.Translate(Vector3.forward * dragonStateMachine.Speed * deltaTime);
        }

        protected void Shooting(int index)
        {
            
            ObjectPoolManager.Instance.ObjectPooling.GetPooledObject(dragonStateMachine.BulletName[index],
                dragonStateMachine.MainProjectile.position);
        }
        
        

        protected IEnumerator WaitToNextShoot(int index, float timeWaitToShoot)
        {
            while (true)
            {
                yield return new WaitForSecondsRealtime(timeWaitToShoot);
                Shooting(index);
            }
        }
    }
}

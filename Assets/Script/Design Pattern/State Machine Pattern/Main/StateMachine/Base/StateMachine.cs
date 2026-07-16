using UnityEngine;

namespace Script.StateMachine.Base
{
    public class StateMachine : MonoBehaviour
    {
        private State currentState { get; set; }

        public void SwitchState(State newState)
        {
            currentState?.Exit();
            currentState = newState;
            currentState?.Enter();
        }
        
        protected void Update()
        {
            currentState?.Tick(Time.deltaTime);
        }
    }
}

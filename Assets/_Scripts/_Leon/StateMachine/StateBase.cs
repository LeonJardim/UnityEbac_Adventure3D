using UnityEngine;

namespace Leon.StateMachine
{
    public class StateBase
    {
        public virtual void OnEnter()
        {
            Debug.Log("State Enter");
        }
        public virtual void OnStay()
        {
            Debug.Log("State Stay");
        }
        public virtual void OnExit()
        {
            Debug.Log("State Exit");
        }
    }
}
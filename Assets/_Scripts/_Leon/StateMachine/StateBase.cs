using UnityEngine;

namespace Leon.StateMachine
{
    public class StateBase
    {
        public virtual void OnEnter(params object[] objs)
        {

        }
        public virtual void OnStay()
        {

        }
        public virtual void OnExit()
        {

        }
    }
}
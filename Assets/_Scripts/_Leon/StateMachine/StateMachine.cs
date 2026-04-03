using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

namespace Leon.StateMachine
{
    public class StateMachine<T> where T : System.Enum
    {
        public Dictionary<T, StateBase> statesDictionary;
        private StateBase _currentState;
        public float timeToStartGame = 1f;

        public StateBase CurrentState
        {
            get { return _currentState; }
        }

        /*
        public StateMachine(T state)
        {
            statesDictionary = new Dictionary<T, StateBase>();
            SwitchState(state);
        }*/

        public void Update()
        {
            if (_currentState != null) _currentState.OnStay();
        }


        public void RegisterStates(T typeEnum, StateBase state)
        {
            statesDictionary.Add(typeEnum, state);
        }

        public void Init()
        {
            statesDictionary = new Dictionary<T, StateBase>();
        }

        public void SwitchState(T state, params object[] objs)
        {
            if (_currentState != null) _currentState.OnExit();

            _currentState = statesDictionary[state];

            _currentState.OnEnter(objs);
        }
    }
}
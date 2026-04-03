using Animation;
using DG.Tweening;
using Leon.StateMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    public enum BossAction
    {
        INIT,
        IDLE,
        WALK,
        ATTACK,
        DEATH
    }

    public class BossBase : MonoBehaviour, IDamageable
    {
        [Header("References")]
        public List<Transform> waypoints;
        private AnimationBase _animationBase;
        private HealthBase _health;

        [Header("Attributes")]
        public float moveSpeed = 50f;
        public int attackAmount = 5;
        public float timeBetweenAttacks = 0.5f;

        [Header("Animation")]
        public float startAnimationDuration = 1.0f;
        public Ease startAnimationEase = Ease.OutBack;
        private Vector3 _initialScale;

        private StateMachine<BossAction> _stateMachine;

        private void Awake()
        {
            _initialScale = transform.localScale;
            transform.localScale = Vector3.zero;
            _animationBase = GetComponent<AnimationBase>();
            _health = GetComponent<HealthBase>();
            if (_health != null) _health.OnKill += OnKill;
            StateMachineInit();
        }

        public void Init()
        {
            SwitchState(BossAction.INIT);
        }

        #region DAMAGEABLE
        protected virtual void OnKill()
        {
            _health.OnKill -= OnKill;
            SwitchState(BossAction.DEATH);
            Destroy(gameObject, 3f);
        }
        public void Damage(int d)
        {
            _health.TakeDamage(d);
        }

        public void Damage(int d, Vector3 dir)
        {
            _health.TakeDamage(d);
        }
        #endregion

        #region WALK
        public void GoToRandomPoint(Action onArrive = null)
        {
            StartCoroutine(GoToPointCoroutine(waypoints[UnityEngine.Random.Range(0, waypoints.Count)], onArrive));
        }

        IEnumerator GoToPointCoroutine(Transform t, Action onArrive = null)
        {
            while(Vector3.Distance(transform.position, t.position) > 1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, t.position, Time.deltaTime * moveSpeed);
                yield return new WaitForEndOfFrame();
            }
            onArrive?.Invoke();
        }
        #endregion

        #region ATTACK
        public void StartAttack(Action onAttackFinished = null)
        {
            StartCoroutine(AttackCoroutine(onAttackFinished));
        }
        IEnumerator AttackCoroutine(Action onAttackFinished = null)
        {
            int attacks = 0;
            while(attacks < attackAmount)
            {
                attacks++;
                yield return new WaitForSeconds(timeBetweenAttacks);
            }
            onAttackFinished?.Invoke();
        }
        #endregion

        #region ANIMATION
        public void StartInitAnimation()
        {
            transform.DOScale(_initialScale, startAnimationDuration).SetEase(startAnimationEase).OnComplete(() =>
            {
                SwitchState(BossAction.WALK);
            });
        }
        public void PlayAnimationByTrigger(AnimationType animationType)
        {
            _animationBase.PlayAnimationByTrigger(animationType);
        }
        #endregion

        #region DEBUG
        [NaughtyAttributes.Button]
        private void SwitchInit()
        {
            SwitchState(BossAction.INIT);
        }
        [NaughtyAttributes.Button]
        private void SwitchWalk()
        {
            SwitchState(BossAction.WALK);
        }
        [NaughtyAttributes.Button]
        private void SwitchAttack()
        {
            SwitchState(BossAction.ATTACK);
        }
        #endregion

        #region STATE MACHINE
        public void SwitchState(BossAction state)
        {
            _stateMachine.SwitchState(state, this);
        }
        private void StateMachineInit()
        {
            _stateMachine = new StateMachine<BossAction>();
            _stateMachine.Init();

            _stateMachine.RegisterStates(BossAction.INIT, new BossStateInit());
            _stateMachine.RegisterStates(BossAction.WALK, new BossStateWalk());
            _stateMachine.RegisterStates(BossAction.ATTACK, new BossStateAttack());
            _stateMachine.RegisterStates(BossAction.DEATH, new BossStateDeath());
        }
        #endregion
    }
}

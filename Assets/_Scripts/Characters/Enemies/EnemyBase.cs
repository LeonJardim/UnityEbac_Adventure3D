using Animation;
using UnityEngine;

namespace Enemy
{
    public class EnemyBase : MonoBehaviour, IDamageable
    {
        protected AnimationBase _animationBase;
        protected Collider _collider;
        protected HealthBase _health;

        protected virtual void Awake()
        {
            _collider = GetComponent<Collider>();
            _animationBase = GetComponent<AnimationBase>();
            _health = GetComponent<HealthBase>();
            if (_health != null) _health.OnKill += OnKill;
        }

        protected virtual void OnKill()
        {
            if (_collider != null) _collider.enabled = false;
            _health.OnKill -= OnKill;
            PlayAnimationByTrigger(AnimationType.DEATH);
            Destroy(gameObject, 2f);
        }

        public void PlayAnimationByTrigger(AnimationType animationType)
        {
            _animationBase.PlayAnimationByTrigger(animationType);
        }

        public void Damage(int d)
        {
            _health.TakeDamage(d);
        }
    }
}
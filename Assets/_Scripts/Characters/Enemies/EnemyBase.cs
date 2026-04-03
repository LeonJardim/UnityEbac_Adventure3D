using Animation;
using DG.Tweening;
using UnityEngine;

namespace Enemy
{
    public class EnemyBase : MonoBehaviour, IDamageable
    {
        public bool lookAtPlayer = false;
        protected AnimationBase _animationBase;
        protected Collider _collider;
        protected HealthBase _health;
        private Player _player;

        protected virtual void Awake()
        {
            _collider = GetComponent<Collider>();
            _animationBase = GetComponent<AnimationBase>();
            _health = GetComponent<HealthBase>();
            if (_health != null) _health.OnKill += OnKill;
        }

        private void Start()
        {
            _player = FindAnyObjectByType<Player>();
        }

        protected virtual void Update()
        {
            if (lookAtPlayer)
            {
                transform.LookAt(_player.transform.position);
            }
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

        private void OnCollisionEnter(Collision collision)
        {
            Player p = collision.transform.GetComponent<Player>();
            if (p != null)
            {
                p.Damage(1);
            }
        }


        public void Damage(int amount)
        {
            _health.TakeDamage(amount);
            transform.position -= transform.forward;
        }
        public void Damage(int amount, Vector3 dir)
        {
            _health.TakeDamage(amount);
            transform.DOMove(transform.position - dir, 0.2f);
        }
    }
}
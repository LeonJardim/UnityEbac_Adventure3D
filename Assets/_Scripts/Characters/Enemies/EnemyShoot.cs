using UnityEngine;

namespace Enemy
{
    public class EnemyShoot : EnemyBase
    {
        public GunBase gun;

        protected override void Awake()
        {
            base.Awake();

            gun.StartShoot();
        }
    }
}
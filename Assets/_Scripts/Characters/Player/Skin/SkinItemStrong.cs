using UnityEngine;

namespace Skin
{
    public class SkinItemStrong : SkinItem
    {
        public float damageMultiplier = 0.5f;

        protected override void Collect()
        {
            if (_collected) return;
            player.ChangeDamageMultiply(damageMultiplier, duration);
            base.Collect();
        }
    }
}
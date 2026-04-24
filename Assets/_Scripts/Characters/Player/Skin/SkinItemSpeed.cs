using UnityEngine;

namespace Skin
{
    public class SkinItemSpeed : SkinItem
    {
        public float speedMultiplier = 2f;

        protected override void Collect()
        {
            if (_collected) return;
            player.ChangeSpeed(speedMultiplier, duration);
            base.Collect();
        }
    }
}
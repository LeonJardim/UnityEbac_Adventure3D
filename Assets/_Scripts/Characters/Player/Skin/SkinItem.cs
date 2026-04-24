using Items;
using UnityEngine;

namespace Skin
{
    public class SkinItem : Collectable
    {
        [Header("Skin Item")]
        public SkinType skinType;
        public float duration;

        protected override void Collect()
        {
            var setup = SkinManager.Instance.GetSetupByType(skinType);
            player.ChangeTexture(setup, duration);

            base.Collect();

        }
    }
}
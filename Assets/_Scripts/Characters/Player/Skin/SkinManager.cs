using Leon.Singleton;
using System.Collections.Generic;
using UnityEngine;

namespace Skin
{
    public enum SkinType
    {
        DEFAULT,
        SPEED,
        STRONG
    }

    public class SkinManager : Singleton<SkinManager>
    {
        public List<SkinSetup> skinSetups;

        public SkinSetup GetSetupByType(SkinType type)
        {
            return skinSetups.Find(i => i.skinType == type);
        }
    }

    [System.Serializable]
    public class SkinSetup
    {
        public SkinType skinType;
        public Texture2D texture;
    }
}

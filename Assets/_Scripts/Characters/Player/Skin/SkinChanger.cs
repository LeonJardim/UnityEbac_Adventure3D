using UnityEngine;
using static UnityEngine.Audio.GeneratorInstance;

namespace Skin
{
    public class SkinChanger : MonoBehaviour
    {
        public SkinnedMeshRenderer mesh;

        private Texture2D _defaultTexture;


        private void Awake()
        {
            _defaultTexture = (Texture2D) mesh.materials[0].GetTexture("_EmissionMap");
        }

        public void ChangeTexture(SkinSetup setup)
        {
            mesh.materials[0].SetTexture("_EmissionMap", setup.texture);
        }

        public void ResetTexture()
        {
            mesh.materials[0].SetTexture("_EmissionMap", _defaultTexture);
        }
    }
}
using UnityEngine;

namespace Items
{
    public class Collectable : MonoBehaviour
    {
        public string tagToCompare = "Player";
        public ParticleSystem particles;
        public GameObject mesh;
        public AudioSource audioSource;
        public float timeToDestroy;
        private bool _collected = false;

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.CompareTag(tagToCompare))
            {
                if (_collected) return;
                Collect();
            }
        }

        protected virtual void Collect()
        {
            if (_collected) return;
            _collected = true;
            OnCollect();
            Invoke(nameof(DestroyMe), timeToDestroy);
        }
        protected virtual void OnCollect()
        {
            if (particles != null) particles.Play();
            if (mesh != null) mesh.SetActive(false);
            if (audioSource != null) audioSource.Play();
        }

        private void DestroyMe()
        {
            Destroy(gameObject);
        }
    }
}
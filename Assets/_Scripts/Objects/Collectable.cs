using UnityEngine;

namespace Items
{
    public class Collectable : MonoBehaviour
    {
        public string tagToCompare = "Player";
        [Header("Effects")]
        public GameObject mesh;
        public ParticleSystem particles;
        public AudioSource audioSource;
        public float timeToDestroy;

        protected Player player;
        protected bool _collected = false;

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.CompareTag(tagToCompare))
            {
                if (_collected) return;
                player = collider.GetComponent<Player>();
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
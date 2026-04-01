using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    public float speed = 50f;
    public float lifeTime = 1.0f;
    public int damage = 5;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.forward);
    }

    private void OnCollisionEnter(Collision collision)
    {
        var damageable = collision.transform.GetComponent<IDamageable>();
        damageable?.Damage(damage);
        Destroy(gameObject);
    }
}

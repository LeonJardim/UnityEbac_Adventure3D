using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    public float speed = 50f;
    public float lifeTime = 1.0f;
    public int damage = 5;
    public List<string> tagsToHit;

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
        foreach (var t in tagsToHit)
        {
            if (collision.transform.CompareTag(t))
            {
                var damageable = collision.transform.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    Vector3 dir = collision.transform.position - transform.position;
                    dir.y = 0f;
                    dir = -dir.normalized;

                    damageable.Damage(damage, dir);
                }
                break;
            }
        }

        Destroy(gameObject);
    }
}

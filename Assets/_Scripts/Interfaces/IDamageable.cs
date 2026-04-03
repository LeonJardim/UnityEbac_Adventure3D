using UnityEngine;

public interface IDamageable
{
    void Damage(int d);
    void Damage(int d, Vector3 dir);
}

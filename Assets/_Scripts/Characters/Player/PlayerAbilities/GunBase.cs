using System;
using System.Collections;
using UnityEngine;

public class GunBase : MonoBehaviour
{
    public ProjectileBase projectile;
    public Transform positionToShoot;
    public float fireRate = 0.3f;
    public float speed = 50f;

    private Coroutine _currentCoroutine;

    public virtual void Shoot()
    {
        var obj = Instantiate(projectile);
        obj.transform.position = positionToShoot.position;
        obj.transform.rotation = positionToShoot.rotation;
        obj.speed = speed;
    }

    protected virtual IEnumerator ShootCoroutine()
    {
        while (true)
        {
            Shoot();
            yield return new WaitForSeconds(fireRate);
        }
    }

    public void StartShoot()
    {
        StopShoot();
        _currentCoroutine = StartCoroutine(ShootCoroutine());
    }
    public void StopShoot()
    {
        if (_currentCoroutine != null)
            StopCoroutine(_currentCoroutine);
    }
}

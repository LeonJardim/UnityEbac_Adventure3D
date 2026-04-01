using UnityEngine;

public class GunShootAngle : GunShootLimit
{
    public int amountPerShoot = 4;
    public float angle = 15f;

    public override void Shoot()
    {
        int mult = 0;

        for (int i = 0; i < amountPerShoot; i++)
        {
            if (i%2 == 0)
            {
                mult++;
            }

            var obj = Instantiate(projectile, positionToShoot);

            obj.transform.localPosition = Vector3.zero;
            obj.transform.localEulerAngles = (i % 2 == 0 ? angle : -angle) * mult * Vector3.up;
            obj.speed = speed;
            obj.transform.parent = null;
        }
    }
}

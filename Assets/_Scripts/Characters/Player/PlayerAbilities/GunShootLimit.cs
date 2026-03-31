using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GunShootLimit : GunBase
{
    public List<UIGunUpdater> uIGunUpdaters;

    public float maxShoots = 5f;
    public float reloadTime = 1f;

    private float _currentShoots;
    private bool _reloading = false;


    protected override IEnumerator ShootCoroutine()
    {
        if (_reloading) yield break;

        while (true)
        {
            if (_currentShoots < maxShoots)
            {
                Shoot();
                _currentShoots++;
                CheckReload();
                UpdateUI();
                yield return new WaitForSeconds(fireRate);
            }

        }
    }

    private void CheckReload()
    {
        if (_currentShoots >= maxShoots)
        {
            StopShoot();
            StartReload();
        }
    }

    private void StartReload()
    {
        _reloading = true;
        StartCoroutine(ReloadCoroutine());
    }

    IEnumerator ReloadCoroutine()
    {
        float time = 0f;
        while(time < reloadTime)
        {
            time += Time.deltaTime;
            uIGunUpdaters.ForEach(i => i.UpdateValue(time/reloadTime));
            yield return new WaitForEndOfFrame();
        }
        _currentShoots = 0;
        _reloading = false;
    }

    private void UpdateUI()
    {
        uIGunUpdaters.ForEach(i => i.UpdateValue(maxShoots, _currentShoots));
    }
}

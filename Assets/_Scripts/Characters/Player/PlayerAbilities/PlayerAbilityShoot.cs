using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityShoot : PlayerAbility
{
    public List<UIUpdater> uIUpdaters;

    public Transform gunPosition;
    public List<GunBase> preFabGuns;
    public List<GunBase> myGuns;

    private GunBase _currentGun;


    protected override void Init()
    {
        base.Init();
        CreateGuns();
    }

    private void CreateGuns()
    {
        foreach (var gun in preFabGuns)
        {
            var _newGun = Instantiate(gun, gunPosition);
            _newGun.transform.localPosition = _newGun.transform.localEulerAngles = Vector3.zero;
            if (_newGun is GunShootLimit g)
            {
                g.uIGunUpdaters = uIUpdaters;
            }
            myGuns.Add(_newGun);
            _newGun.enabled = false;
        }
        SwitchGun(0);
    }

    public void SwitchGun(int i)
    {
        if (myGuns[i] == _currentGun) return;
        if (_currentGun) _currentGun.enabled = false;
        _currentGun = myGuns[i];
        _currentGun.enabled = true;
    }

    public void StartShoot()
    {
        _currentGun.StartShoot();
    }
    public void StopShoot()
    {
        _currentGun.StopShoot();
    }
}

using System;
using UnityEngine;

public class HealthBase : MonoBehaviour
{
    public Action OnKill;
    public int startingLife = 10;
    public float delayToKill = 1f;

    private FlashColor _flashColor;
    private int _currentLife;
    public bool _isDead = false;


    private void Awake()
    {
        _isDead = false;
        _currentLife = startingLife;
        if (_flashColor == null)
        {
            _flashColor = GetComponent<FlashColor>();
        }
    }

    public void TakeDamage(int damage)
    {
        if (_isDead) return;

        _currentLife -= damage;
        if (_currentLife <= 0 )
        {
            Kill();
        }

        if (_flashColor != null)
        {
            _flashColor.Flash();
        }
    }

    private void Kill()
    {
        _isDead = true;
        OnKill?.Invoke();
    }
}

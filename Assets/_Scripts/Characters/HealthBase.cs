using System;
using UnityEngine;

public class HealthBase : MonoBehaviour
{
    public Action OnKill;
    public int startingLife = 10;
    public float delayToKill = 1f;
    public bool isDead = false;

    [SerializeField] private FlashColor _flashColor;
    [SerializeField] private ParticleSystem _particleSystem;
    private int _currentLife;


    private void Awake()
    {
        ResetLife();
        _flashColor = GetComponentInChildren<FlashColor>();
        _particleSystem = GetComponentInChildren<ParticleSystem>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        _currentLife -= damage;
        if (_currentLife <= 0 )
        {
            Kill();
        }

        if (_flashColor != null) _flashColor.Flash();
        if (_particleSystem != null) _particleSystem.Play();
    }

    public void ResetLife()
    {
        isDead = false;
        _currentLife = startingLife;
    }

    private void Kill()
    {
        isDead = true;
        OnKill?.Invoke();
    }
}

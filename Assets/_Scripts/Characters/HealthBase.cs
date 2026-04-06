using System;
using System.Collections.Generic;
using UnityEngine;

public class HealthBase : MonoBehaviour
{
    public Action OnKill;
    public int startingLife = 10;
    public bool isDead = false;

    [SerializeField] private List<UIUpdater> _uiUpdaters;
    [SerializeField] private List<FlashColor> _flashColors;
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

        _uiUpdaters?.ForEach(i => i.UpdateValue((float)_currentLife / startingLife));
        if (_flashColor != null) _flashColor.Flash();
        _flashColors?.ForEach(i => i.Flash());
        if (_particleSystem != null) _particleSystem.Play();
    }

    public void ResetLife()
    {
        isDead = false;
        _currentLife = startingLife;
        _uiUpdaters?.ForEach(i => i.UpdateValue(1f));
    }

    private void Kill()
    {
        isDead = true;
        OnKill?.Invoke();
    }
}

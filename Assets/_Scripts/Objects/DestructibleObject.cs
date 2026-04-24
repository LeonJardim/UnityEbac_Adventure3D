using DG.Tweening;
using UnityEngine;

public class DestructibleObject : MonoBehaviour, IDamageable
{
    public HealthBase health;
    public Transform dropPoint;
    [Header("Coins")]
    public GameObject coinObject;
    public int coinsToDrop = 2;

    [Header("Animation")]
    public float shakeDuration = 0.2f;
    public float shakeForce = 1f;
    private Tween _currTween;

    private void OnValidate()
    {
        if (health == null) health = GetComponent<HealthBase>();
    }

    private void Awake()
    {
        if (health == null) health = GetComponent<HealthBase>();
        health.OnKill += OnKill;
    }

    public void Damage(int d = 1)
    {
        health.TakeDamage(d);
        DropCoins();
        _currTween?.Kill(true);
        _currTween = transform.DOShakeScale(shakeDuration, shakeForce);
    }

    public void Damage(int d, Vector3 dir)
    {
        health.TakeDamage(d);
        DropCoins();
        _currTween?.Kill(true);
        _currTween = transform.DOShakeScale(shakeDuration, shakeForce);
    }

    private void OnKill()
    {
        health.OnKill -= OnKill;
        Destroy(gameObject);
    }

    private void DropCoins()
    {
        for (int i = 0; i < coinsToDrop; i++)
        {
            var o = Instantiate(coinObject);
            o.transform.position = dropPoint.position;
            o.transform.DOScale(0, 0.2f).SetEase(Ease.OutBack).From();
        }
    }
}

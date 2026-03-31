using UnityEngine;
using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;
using DG.Tweening;

public class FlashColor : MonoBehaviour
{
    [SerializeField] private List<SpriteRenderer> sprites;
    public Color flashColor = Color.red;
    public float flashDuration = 0.15f;
    private Tween _currentTween;

    private void OnValidate()
    {
        sprites = new List<SpriteRenderer>();
        foreach(var child in transform.GetComponentsInChildren<SpriteRenderer>())
        {
            sprites.Add(child);
        }
    }

    public void Flash()
    {
        if (_currentTween != null)
        {
            _currentTween.Kill();
            sprites.ForEach(i => i.color = Color.white);
        }
        foreach(var s in sprites)
        {
            _currentTween = s.DOColor(flashColor, flashDuration).SetLoops(2, LoopType.Yoyo);
        }
    }
}

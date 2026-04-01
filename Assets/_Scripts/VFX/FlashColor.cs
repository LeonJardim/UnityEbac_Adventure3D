using UnityEngine;
using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;
using DG.Tweening;

public class FlashColor : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public Color flashColor = Color.red;
    public float flashDuration = 0.15f;

    private Tween _currentTween;
    private Color _defaultColor;


    private void Start()
    {
        _defaultColor = meshRenderer.material.GetColor("_EmissionColor");
    }

    private void OnValidate()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }


    [NaughtyAttributes.Button]
    public void Flash()
    {
        if (!_currentTween.IsActive())
        {
            _currentTween = meshRenderer.material.DOColor(flashColor, "_EmissionColor", flashDuration).SetLoops(2, LoopType.Yoyo);
        }
    }
}

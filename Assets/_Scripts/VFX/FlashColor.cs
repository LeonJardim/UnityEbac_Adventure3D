using UnityEngine;
using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;
using DG.Tweening;

public class FlashColor : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public Color flashColor = Color.red;
    public float flashDuration = 0.15f;

    private Tween _currentTween;
    private Color _defaultColor;


    private void Start()
    {
        if (meshRenderer != null)
            _defaultColor = meshRenderer.material.GetColor("_EmissionColor");
        else if (skinnedMeshRenderer != null)
            _defaultColor = skinnedMeshRenderer.material.GetColor("_EmissionColor");
    }

    private void OnValidate()
    {
        if(meshRenderer == null) meshRenderer = GetComponent<MeshRenderer>();
        if (skinnedMeshRenderer == null) skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
    }


    [NaughtyAttributes.Button]
    public void Flash()
    {
        if (meshRenderer != null && !_currentTween.IsActive())
        {
            _currentTween = meshRenderer.material.DOColor(flashColor, "_EmissionColor", flashDuration).SetLoops(2, LoopType.Yoyo);
        }
        if (skinnedMeshRenderer != null && !_currentTween.IsActive())
        {
            _currentTween = skinnedMeshRenderer.material.DOColor(flashColor, "_EmissionColor", flashDuration).SetLoops(2, LoopType.Yoyo);
        }
    }
}

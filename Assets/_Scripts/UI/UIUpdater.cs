using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIUpdater : MonoBehaviour
{
    public Image uiImage;

    [Header("Animation")]
    public float duration = 0.1f;
    public Ease ease = Ease.OutBack;

    private Tween _tween;

    private void OnValidate()
    {
        if(uiImage == null) uiImage = GetComponent<Image>();
    }

    public void UpdateValue(float f)
    {
        uiImage.fillAmount = f;
    }

    public void UpdateValue(float max, float current)
    {
        if (_tween != null) _tween.Kill(true);
        _tween = uiImage.DOFillAmount(1 - (current / max), duration).SetEase(ease);
    }
}

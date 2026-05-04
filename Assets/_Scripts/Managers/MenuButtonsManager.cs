using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class MenuButtonsManager : MonoBehaviour
{
    public List<GameObject> buttons;

    [Header("Animation")]
    [SerializeField] private float duration = 0.2f;
    [SerializeField] private float delay = 0.1f;
    [SerializeField] private Ease ease = Ease.OutBack;


    private void Start()
    {
        HideButtons();
        ShowButtons();
    }

    private void ShowButtons()
    {
        for(int i = 0; i < buttons.Count; i++)
        {
            var b = buttons[i];
            b.SetActive(true);
            b.transform.DOScale(1, duration).SetDelay(i * delay).SetEase(ease);
        }
    }

    private void HideButtons()
    {
        foreach (var b in buttons)
        {
            b.transform.localScale = Vector3.zero;
            b.SetActive(false);
        }
    }

}

using DG.Tweening;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    public GameObject icon;
    private Animator _animator;
    private string _openTrigger = "Open";
    private string _closeTrigger = "Close";
    private bool _open = false;
    private float _startScale;

    private void OnValidate()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _startScale = icon.transform.localScale.x;
        icon.transform.localScale = Vector3.zero;
    }

    public void Action()
    {
        if (!_open)
        {
            OpenChest();
        }
        else
        {
            CloseChest();
        }
    }

    private void OpenChest()
    {
        _animator.SetTrigger(_openTrigger);
        _open = true;
    }
    private void CloseChest()
    {
        _animator.SetTrigger(_closeTrigger);
        _open = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Player p = other.GetComponent<Player>();
        if (p != null)
        {
            icon.transform.DOScale(_startScale, 0.2f).SetEase(Ease.OutBack);
            p.interactableObj = this;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Player p = other.GetComponent<Player>();
        if (p != null)
        {
            icon.transform.DOScale(0, 0.2f).SetEase(Ease.InBack);
            if (p.interactableObj == (IInteractable)this)
            {
                p.interactableObj = null;
            }
        }
    }
}

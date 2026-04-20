using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Items
{
    public class ItemLayout : MonoBehaviour
    {
        public Image uiIcon;
        public TextMeshProUGUI uiText;
        public TextMeshProUGUI itemTips;

        private Sequence _tweenSequence;

        public void UpdateUI(ItemSetup setup)
        {
            uiIcon.sprite = setup.icon;
            uiText.text = setup.soInt.Value.ToString();

            _tweenSequence?.Kill(true);
            _tweenSequence = DOTween.Sequence();

            _tweenSequence.Append(gameObject.transform.DOScale(1, 0.2f).SetEase(Ease.OutBack));
            _tweenSequence.AppendInterval(3f);
            _tweenSequence.Append(gameObject.transform.DOScale(0, 0.2f).SetEase(Ease.InBack));
        }

        public void ShowItemTip()
        {
            itemTips.transform.DOScale(1, 0.2f).SetEase(Ease.OutBack);
            Invoke(nameof(HideItemTip), 3f);
        }
        private void HideItemTip()
        {
            itemTips.transform.DOScale(0, 0.2f).SetEase(Ease.InBack);
        }
    }
}
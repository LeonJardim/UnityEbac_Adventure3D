using DG.Tweening;
using Items;
using System.Collections.Generic;
using UnityEngine;

public class ChestItemCoin : ChestItem
{
    public GameObject coinObject;
    public int coinNumber = 5;
    [Space] 
    public Vector2 spawnRange = new(-2f, 2f);

    private List<GameObject> _items = new();

    public override void ShowItem()
    {
        base.ShowItem();
        CreateItems();
    }
    public override void Collect()
    {
        base.Collect();
        foreach(var i in _items)
        {
            i.transform.DOMoveY(2f, 1f).SetRelative();
            i.transform.DOScale(0, 0.5f).SetDelay(0.5f)
                .OnComplete(() =>
                {
                    ItemManager.Instance.AddByType(ItemType.COIN);
                    Destroy(i);
                    _items.Remove(i);
                });
        }
    }

    private void CreateItems()
    {
        for (int i = 0; i < coinNumber; i++)
        {
            var item = Instantiate(coinObject);
            item.transform.position = transform.position + Vector3.forward * Random.Range(spawnRange.x, spawnRange.y)
                + Vector3.right * Random.Range(spawnRange.x, spawnRange.y);
            item.transform.DOScale(0, 0.3f).SetEase(Ease.OutBack).From();
            _items.Add(item);
        }
    }
}

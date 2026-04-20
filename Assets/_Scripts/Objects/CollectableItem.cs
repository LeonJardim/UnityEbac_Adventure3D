using UnityEngine;
using Items;

public class CollectableItem : Collectable
{
    public ItemType myType;
    protected override void OnCollect()
    {
        base.OnCollect();
        ItemManager.Instance.AddByType(myType);
    }
}

using Leon.Singleton;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public enum ItemType
    {
        COIN,
        LIFE_PACK
    }


    public class ItemManager : Singleton<ItemManager>
    {
        public ItemLayout itemsUI;
        public List<ItemSetup> itemSetups;


        private void Start()
        {
            Reset();
        }

        private void Reset()
        {
            foreach (var i in itemSetups)
            {
                i.soInt.Value = 0;
            }
        }


        public ItemSetup GetItemByType(ItemType type)
        {
            return itemSetups.Find(i => i.itemType == type);
        }

        public void AddByType(ItemType itemType, int amount = 1)
        {
            if (amount < 0) return;

            var item = itemSetups.Find(i => i.itemType == itemType);
            item.soInt.Value += amount;

            UpdateUI(item);
        }
        public void RemoveByType(ItemType itemType, int amount = 1)
        {
            if (amount < 0) return;

            var item = itemSetups.Find(i => i.itemType == itemType);
            item.soInt.Value -= amount;

            if (item.soInt.Value < 0) item.soInt.Value = 0;

            UpdateUI(item);
        }

        private void UpdateUI(ItemSetup itemSetup)
        {
            itemsUI.UpdateUI(itemSetup);
        }

        public void ShowItemTip()
        {
            itemsUI.ShowItemTip();
            UpdateUI(itemSetups.Find(i => i.itemType == ItemType.LIFE_PACK));
        }
    }


    [System.Serializable]
    public class ItemSetup
    {
        public ItemType itemType;
        public SOInt soInt;
        public Sprite icon;
    }
}
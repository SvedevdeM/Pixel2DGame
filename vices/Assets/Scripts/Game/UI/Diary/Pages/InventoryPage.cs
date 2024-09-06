using TMPro;
using UnityEngine;

namespace Vices.Scripts
{
    public class InventoryPage : DiaryPage
    {
        [SerializeField] private InventoryInfo _inventoryInfo;
        [SerializeField] private InventoryItemSlotUI[] _slots;
        [SerializeField] private TMP_Text _itemName;
        [SerializeField] private TMP_Text _itemDescription;

        protected override void OpenPage()
        {
            ClosePage();
            int index = 0;
            foreach (var (key, value) in _inventoryInfo._inventoryItems)
            {
                _slots[index].Show(value);
                index++;
            }
        }

        protected override void ClosePage()
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                _slots[i].Hide();
            }

            _itemName.text = "";
            _itemDescription.text = "";
        }
    }
}

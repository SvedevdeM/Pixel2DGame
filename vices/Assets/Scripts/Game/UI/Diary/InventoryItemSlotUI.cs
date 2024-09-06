using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Vices.Scripts
{
    public class InventoryItemSlotUI : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private Image _itemIcon;
        [SerializeField] private TMP_Text _amountText;
        [SerializeField] private TMP_Text _itemName;
        [SerializeField] private TMP_Text _itemDescription;

        private InventoryItem _inventoryItem;

        public void Show(InventoryItem item)
        {
            gameObject.SetActive(true);
            _itemIcon.sprite = item.Item.Sprite;
            _amountText.text = item.Amount.ToString();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            _itemIcon.sprite = null;
            _amountText.text = "";
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _itemName.text = _inventoryItem.Item.Name;
            _itemDescription.text = _inventoryItem.Item.Description;
        }
    }
}

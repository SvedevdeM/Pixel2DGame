using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Vices.Scripts
{
    public class CloseButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] private Image _closeButtonImage;
        [SerializeField] private Sprite _idle;
        [SerializeField] private Sprite _hover;
        [SerializeField] private Sprite _selected;

        [SerializeField] private UnityEvent _OnClick;

        public void OnPointerEnter(PointerEventData eventData)
        {
            _closeButtonImage.sprite = _hover;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _closeButtonImage.sprite = _idle;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _closeButtonImage.sprite = _selected;

            _OnClick?.Invoke();
        }
    }
}

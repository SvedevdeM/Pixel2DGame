using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Vices.Scripts
{
    public class LabelButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] private Image _onSelect;
        [SerializeField] private Image _onHover;
        [SerializeField] private Image _idle;

        [SerializeField] private UnityEvent _OnClick;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_onSelect.IsActive()) return;
            _onSelect.gameObject.SetActive(false);
            _onHover.gameObject.SetActive(true);
            _idle.gameObject.SetActive(false);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_onSelect.IsActive()) return;
            _onSelect.gameObject.SetActive(false);
            _onHover.gameObject.SetActive(false);
            _idle.gameObject.SetActive(true);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _onSelect.gameObject.SetActive(true);
            _onHover.gameObject.SetActive(false);
            _idle.gameObject.SetActive(false);

            _OnClick?.Invoke();
        }

        public void RefreshImage()
        {
            _onSelect.gameObject.SetActive(false);
            _onHover.gameObject.SetActive(false);
            _idle.gameObject.SetActive(true);
        }
    }
}
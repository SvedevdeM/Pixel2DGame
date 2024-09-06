using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using Vices.Scripts.Core;

namespace Vices.Scripts.Game.UI
{
    public class BlackScreen : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _image;
        [SerializeField] private TMP_Text _deathMessage;
        [SerializeField] private TMP_Text _deathButton;
        [SerializeField] private float _speed = 0.35f;

        public static BlackScreen Screen;

        private void Awake()
        {
            Screen = this;
        }

        public void Show(Action action = null)
        {
            _deathMessage.gameObject.SetActive(false);
            _deathButton.gameObject.SetActive(false);
            gameObject.SetActive(true);
            _image.DOFade(1f, _speed).OnComplete(action.Invoke);
        }

        public void Hide(Action action = null)
        {
            _deathMessage.gameObject.SetActive(false);
            _deathButton.gameObject.SetActive(false);
            _image.DOFade(0f, _speed).OnComplete(() => { action?.Invoke(); gameObject.SetActive(false); });
        }

        public void ShowDeathScreen(Action action = null)
        {
            _deathMessage.gameObject.SetActive(true);
            _deathButton.gameObject.SetActive(true);

            gameObject.SetActive(true);
            _image.DOFade(1f, _speed);
            _deathMessage.DOFade(1f, _speed);
            _deathButton.DOFade(1f, _speed);
        }

        public void Retry()
        {
            SceneSystem.Singleton.LoadScene("Campus", () => Hide());
        }
    }
}

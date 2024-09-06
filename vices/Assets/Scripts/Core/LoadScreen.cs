using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Vices.Scripts.Core
{
    public class LoadScreen : MonoBehaviour
    {
        [SerializeField] private Image _blackScreen;
        [SerializeField] private TMP_Text _text;

        private Color _showColor = new Color(255, 255, 255, 255);
        private Color _hideColor = new Color(255, 255, 255, 0);

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            gameObject.SetActive(false);
        }

        public void Show(Action onEnd)
        {
            _blackScreen.DOFade(1f, 1f).OnComplete(onEnd.Invoke);
            _text.DOFade(1, 0.75f);

            gameObject.SetActive(true);
        }

        public void Hide(Action onEnd)
        {
            _blackScreen.DOFade(0f, 1f).OnComplete(() => { onEnd?.Invoke(); gameObject.SetActive(false); });
            _text.DOFade(0f, 0.75f);
        }
    }
}
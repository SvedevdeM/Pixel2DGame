using UnityEngine;
using UnityEngine.UI;

public class HealthSliderUI : MonoBehaviour
{
    [SerializeField] private HealthInfo _healthInfo;
    [SerializeField] private Image _image;

    private void Start()
    {
        _healthInfo.SubscribeHealthChanged(OnHealthChange);
    }

    private void OnHealthChange(float health, float maxHealth)
    {
        _image.fillAmount = maxHealth / health * 0.1f;
    }    
}
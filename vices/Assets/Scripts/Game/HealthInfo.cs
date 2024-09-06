using System;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(HealthInfo), menuName = "JabberwockyWorld/Game/" + nameof(HealthInfo))]
public class HealthInfo : ScriptableObject
{
    public Action<float, float> OnHealthChanged;
    public Action OnDeath;

    public void SubscribeHealthChanged(Action<float, float> changed)
    {
        OnHealthChanged += changed;
    }

    public void SubscribeOnDeath(Action death)
    {
        OnDeath += death;
    }

    public void UnsubscribeHealthChanged(Action<float, float> changed)
    {
        OnHealthChanged -= changed;
    }

    public void UnsubscribeOnDeath(Action death)
    {
        OnDeath -= death;
    }

    public void InvokeDeath()
    {

        OnDeath?.Invoke();
    }

    public void HealthChanged(float currentHealth, float maxHealth)
    {
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
}

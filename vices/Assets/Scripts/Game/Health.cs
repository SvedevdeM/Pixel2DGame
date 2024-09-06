using UnityEngine;

namespace Vices.Scripts.Game
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private HealthInfo healthInfo;
        [SerializeField] private float _maxHealth = 100f;
        private float _health = 100.0f;
        private bool _isDead;

        public void DealDamage(float value) 
        {
            _health -= value;
            healthInfo.HealthChanged(_health, _maxHealth);

            CheckDeath();
        }

        public void Heal(float value)
        {
            _health += value;
            healthInfo.HealthChanged(_health, _maxHealth);
        }

        private void CheckDeath()
        {
            if (_isDead) return;
            if (_health > 0) return;
            healthInfo.InvokeDeath();

            _isDead = true;
        }
    }
}

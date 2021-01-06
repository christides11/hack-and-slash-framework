using UnityEngine;

namespace TDAction.Entities
{
    public class HealthManager : MonoBehaviour
    {
        public delegate void HealthChangedAction(HealthManager source, float oldHealth, float currentHealth);
        public event HealthChangedAction OnHurt;
        public event HealthChangedAction OnHeal;
        public event HealthChangedAction OnHealthSet;

        public float Health { get { return health; } }
        public float MaxHealth { get { return maxHealth; } }

        [SerializeField] private float maxHealth;
        [SerializeField] private float health;

        public void SetMaxHealth(float value)
        {
            maxHealth = value;
        }

        public void SetHealth(float value)
        {
            float oldHealth = health;
            health = value;
            OnHealthSet?.Invoke(this, oldHealth, health);
        }

        public void Hurt(float value)
        {
            health -= value;
            OnHurt?.Invoke(this, health + value, health);
        }

        public void Heal(float value)
        {
            health += value;
            OnHeal?.Invoke(this, health - value, health);
        }
    }
}
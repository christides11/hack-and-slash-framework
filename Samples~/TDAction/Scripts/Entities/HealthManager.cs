using UnityEngine;

namespace TDAction.Entities
{
    public class HealthManager : MonoBehaviour
    {
        public delegate void HealthChangedAction(GameObject gameObject, float oldHealth, float currentHealth);
        public event HealthChangedAction OnHurt;
        public event HealthChangedAction OnHeal;
        public event HealthChangedAction OnHealthSet;

        public float Health { get { return health; } }

        [SerializeField] private float health;

        public void SetHealth(float value)
        {

            float oldHealth = health;
            health = value;
            OnHealthSet?.Invoke(gameObject, oldHealth, health);
        }

        public void Hurt(float value)
        {
            health -= value;
            OnHurt?.Invoke(gameObject, health + value, health);
        }

        public void Heal(float value)
        {
            health += value;
            OnHeal?.Invoke(gameObject, health - value, health);
        }
    }
}
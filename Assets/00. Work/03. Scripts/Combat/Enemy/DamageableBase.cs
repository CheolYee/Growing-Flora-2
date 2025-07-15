
using _00._Work._03._Scripts.Combat.Skills;
using UnityEngine;
using UnityEngine.Events;

namespace _00._Work._03._Scripts.Combat.Enemy
{
    public abstract class DamageableBase : MonoBehaviour, IDamageable
    {
        [Header("Health Settings")] 
        [field:SerializeField] public float MaxHealth {get; private set;}
        
        [field:SerializeField] public float CurrentHealth {get; private set;}
        
        public UnityEvent onHitEvent;
        public UnityEvent onDeadEvent;
        
        
        public virtual void Initialize()
        {
            CurrentHealth = MaxHealth;
        }
        public void TakeDamage(DamageInfo damageInfo)
        {
            CurrentHealth -= damageInfo.damage;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
            onHitEvent?.Invoke();
            if (CurrentHealth <= 0)
            {
                onDeadEvent?.Invoke();
            }
        }
    }
}
using UnityEngine;


public struct DamageInfo
{
    public float damage;
    public float criticalMultiplier;
    public bool isCritical;
    public GameObject attacker;

    public DamageInfo(float damage, float critMult = 1f, bool isCrit = false, GameObject attacker = null)
    {
        this.damage = damage;
        this.criticalMultiplier = critMult;
        this.isCritical = isCrit;
        this.attacker = attacker;
    }
}

namespace _00._Work._03._Scripts.Combat.Skills
{
    public interface IDamageable
    {
        void TakeDamage(DamageInfo damage);
    }
}
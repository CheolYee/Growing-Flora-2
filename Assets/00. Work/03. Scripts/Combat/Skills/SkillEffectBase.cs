using System.Collections;
using _00._Work._03._Scripts.Managers;
using _00._Work._03._Scripts.SO.Skill;
using UnityEngine;

namespace _00._Work._03._Scripts.Combat.Skills
{
    public abstract class SkillEffectBase : MonoBehaviour, IPoolable
    {
        public string ItemName => gameObject.name;
        public GameObject GameObject => gameObject;

        protected bool IsActive;
        protected SkillSo SkillData;
        protected Transform Caster;

        [Header("Optional Components")]
        [SerializeField] protected Animator animator;

        public virtual void ResetItem()
        {
            SkillData = null;
            Caster = null;
        }

        public void Initialize(SkillSo data, Transform caster)
        {
            SkillData = data;
            Caster = caster;
        }

        public virtual void StartEffect(Vector3 targetPos)
        {
            StartCoroutine(EffectFlow(targetPos));
        }


        private IEnumerator EffectFlow(Vector3 targetPos)
        {
            transform.position = Caster.position;
            // 1. 캐스팅(준비) 단계
            if (SkillData.castTime > 0 && !string.IsNullOrEmpty(SkillData.castAnimName))
            {
                PlayAnimation(SkillData.castAnimName);
                yield return new WaitForSeconds(SkillData.castTime);
            }

            // 2. Idle 대기
            if (SkillData.preDelay > 0 && !string.IsNullOrEmpty(SkillData.idleAnimName))
            {
                PlayAnimation(SkillData.idleAnimName);
                yield return new WaitForSeconds(SkillData.preDelay);
            }
            
            // 2.5. 만약 떨어지는 모션이 있다면 실행 후 발사단계로
            if (SkillData.useDropEffect)
            {
                PlayAnimation(SkillData.fallAnimName);        // 떨어지는 애니메이션
                yield return StartCoroutine(OnDropPhase(targetPos)); // 낙하 연출
            }

            // 3. 발사 단계
            if (!string.IsNullOrEmpty(SkillData.activeAnimName))
            {
                PlayAnimation(SkillData.activeAnimName);
                OnActivePhase(targetPos);
                yield return new WaitForSeconds(SkillData.activeDuration);
            }
            
            // 4. 종료
            if (SkillData.postDelay > 0 && !string.IsNullOrEmpty(SkillData.endAnimName))
            {
                PlayAnimation(SkillData.endAnimName);
                yield return new WaitForSeconds(SkillData.postDelay);
            }

            EndEffect();
        }
        
        protected virtual IEnumerator OnDropPhase(Vector3? targetPos)
        {
            yield break;
        }

        protected virtual void OnActivePhase(Vector3? targetPos)
        {
            // 각 스킬에서 override 하여 처리
        }

        protected virtual void PlayAnimation(string animName)
        {
            if (animator != null && !string.IsNullOrEmpty(animName))
            {
                animator.Play(animName);
            }
        }

        protected virtual void EndEffect()
        {
            IsActive = false;
            PoolManager.Instance.Push(this);
            gameObject.SetActive(false);
        }

        protected virtual void HandleCollision(Collider2D target)
        {
            if ( target != null && target.TryGetComponent(out IDamageable damageable))
            {
                var damage = new DamageInfo(SkillData.damage, 1, false, this.GameObject);
                damageable.TakeDamage(damage);
            }
        }
    }
}

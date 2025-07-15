using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _00._Work._03._Scripts.Combat.Skills
{
    public class BoxAreaEffect : SkillEffectBase
    {
        [Header("BoxArea Settings")]
        public int hitCount = 1;
        public float hitDelay = 1;
        public Vector2 hitboxSize = new(3f, 1f);
        public Vector3 offset;
        public LayerMask targetLayer;

        private Vector3 _targetPosition;
        private readonly HashSet<Collider2D> _alreadyHit = new();
        private float _elapsedTime;
        
        protected override void OnActivePhase(Vector3? targetPos)
        {
            _alreadyHit.Clear();
            _elapsedTime = 0f;
            _targetPosition = targetPos ?? Caster.position;
            
            transform.position = _targetPosition;
            IsActive = true;
        }
        protected override IEnumerator OnDropPhase(Vector3? targetPos)
        {
            Vector3 start = (targetPos ?? Caster.position) + SkillData.dropStartOffset;
            Vector3 end = targetPos ?? Caster.position;

            transform.position = start;

            float elapsed = 0f;
            while (elapsed < SkillData.dropDuration)
            {
                float t = elapsed / SkillData.dropDuration;
                transform.position = Vector3.Lerp(start, end, t);
                elapsed += Time.deltaTime;
                yield return null;
            }

            // 떨어지고 나서 공격 처리
            IsActive = true;
        }
        

        private void Update()
        {
            if (!IsActive) return;
            
            _elapsedTime += Time.deltaTime;

            var hits = Physics2D.OverlapBoxAll(transform.position + offset, hitboxSize, 0f, targetLayer);
            foreach (var hit in hits)
            {
                if (!_alreadyHit.Add(hit)) continue;

                StartCoroutine(HitCoroutine(hit));
            }

            if (_elapsedTime >= SkillData.activeDuration)
            {
                IsActive = false;
            }
        }

        private IEnumerator HitCoroutine(Collider2D hit)
        {
            for (int i = 0; i < hitCount; i++)
            {
                HandleCollision(hit);
                yield return new WaitForSeconds(hitDelay);
            }
        }

        public override void ResetItem()
        {
            base.ResetItem();
            _alreadyHit.Clear();
            _elapsedTime = 0f;
            IsActive = false;
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position + offset, hitboxSize);
        }
#endif
    }
}
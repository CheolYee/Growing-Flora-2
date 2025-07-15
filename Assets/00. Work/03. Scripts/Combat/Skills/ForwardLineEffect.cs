using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _00._Work._03._Scripts.Combat.Skills
{
    public class ForwardLineEffect : SkillEffectBase
    {
        [Header("Forward Line Settings")]
        public int hitCount = 1;
        public float hitDelay = 1;
        public float moveSpeed = 10f;
        public Vector2 hitboxSize = new(3f, 1f);
        public Vector3 offset;
        public LayerMask targetLayer;

        private Vector3 _direction;
        private float _elapsedTime;
        private readonly HashSet<Collider2D> _alreadyHit = new();
        private bool _isMoving = false;

        protected override void OnActivePhase(Vector3? targetPos)
        {
            Vector3 casterPos = Caster.position;

            // 시작 위치 고정
            transform.position = casterPos;

            // 기본 방향은 오른쪽
            _direction = Vector3.right;

            if (targetPos.HasValue)
            {
                // 정확히 캐스터를 기준으로 방향을 계산해야 함
                _direction = (targetPos.Value - casterPos).normalized;
            }

            // 이펙트가 향하는 방향으로 회전 (2D에서는 z축 기준 회전)
            /*float angleZ = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angleZ);*/

            _alreadyHit.Clear();
            _elapsedTime = 0f;
            _isMoving = true;
        }


        private void Update()
        {
            if (!_isMoving) return;

            transform.position += _direction * (moveSpeed * Time.deltaTime);
            _elapsedTime += Time.deltaTime;

            var hits = Physics2D.OverlapBoxAll(transform.position + offset, hitboxSize, 0f, targetLayer);
            foreach (var hit in hits)
            {
                if (!_alreadyHit.Add(hit)) continue;

                StartCoroutine(HitCoroutine(hit));
            }

            if (_elapsedTime >= SkillData.activeDuration)
            {
                _isMoving = false;
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
            _isMoving = false;
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

using System.Collections;
using UnityEngine;

namespace _00._Work._03._Scripts.Combat.Enemy
{
    public class Enemy : DamageableBase
    {
        [SerializeField] public SpriteRenderer spriteRenderer;
        [SerializeField] private float feedbackDuration = 0.1f;
        
        private Material _material;
        
        private readonly int _isBlinkHash = Shader.PropertyToID("_IsBlink");

        private void Awake()
        {
            _material = spriteRenderer.material;
        }

        private void Start()
        {
            Initialize();
        }
        
        public void HitFeedBack()
        {
            if (spriteRenderer != null)
            {
                _material.SetInt(_isBlinkHash, 1);
                StartCoroutine(DelayBlinkCoroutine());
            }
        }
        private IEnumerator DelayBlinkCoroutine()
        {
            yield return new WaitForSeconds(feedbackDuration);
            _material.SetInt(_isBlinkHash, 0);
        }

        public void IsDead()
        {
            Destroy(gameObject);
        }
        
    }
}
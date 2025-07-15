using System.Collections.Generic;
using _00._Work._02._Scripts.Manager;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace _00._Work._03._Scripts.Combat.Cost
{
    public class CostManager : MonoSingleton<CostManager>
    {
        [Header("Cost Settings")] 
        public int maxCost = 10;
        public float increaseInterval = 1f;
        
        private int _currentCost;
        private float _timer;
        
        [Header("UI")]
        public List<Image> costImgs;
        public TextMeshProUGUI costText;

        private void Start()
        {
            _currentCost = 0;
            _timer = 0;

            foreach (var slot in costImgs)
            {
                slot.fillAmount = 0;
            }
            
        }

        private void Update()
        {
            if (_currentCost >= maxCost) return;

            _timer += Time.deltaTime;
            float progress = _timer / increaseInterval;

            // 현재 충전 중인 칸은 _currentCost 기준
            costImgs[_currentCost].fillAmount = Mathf.Clamp01(progress);

            if (_timer >= increaseInterval)
            {
                _timer = 0f;

                // 충전 완료된 칸에 애니메이션
                costImgs[_currentCost].fillAmount = 1f;
                PlayingSliderAnim(costImgs[_currentCost].transform);

                // 코스트 수치 증가
                AddCost(1);
                UpdateUI();
            }

            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                AddCost(10);
            }
        }

        public void UseCost(int amount)
        {
            if (_currentCost < amount) return;

            _currentCost -= amount;
            
            // 차감
            for (int i = 0; i < costImgs.Count; i++)
            {
                if (i < _currentCost)
                {
                    costImgs[i].fillAmount = 1f; // 이미 찬 슬롯
                }
                else
                {
                    costImgs[i].fillAmount = 0f; // 남은 슬롯은 초기화
                }
            }

            costText.text = _currentCost.ToString();
            PlayCostTextPopAnim();
        }

        private void AddCost(int amount)
        {
            _currentCost = Mathf.Min(_currentCost + amount, maxCost);
            UpdateUI();
        }

        private void PlayingSliderAnim(Transform target)
        {
            target.DOKill(); // 기존 트윈 초기화
            target.localScale = Vector3.one; // 스케일 초기화

            target.DOScale(1.1f, 0.1f)
                .SetEase(Ease.OutBack)
                .OnComplete(() =>
                {
                    target.DOScale(1f, 0.1f).SetEase(Ease.InQuad);
                });
        }
        
        private void PlayCostTextPopAnim()
        {
            Transform textTransform = costText.transform;

            textTransform.DOKill(); // 기존 애니메이션 초기화
            textTransform.localScale = Vector3.one; // 스케일 초기화

            textTransform.DOScale(1.2f, 0.1f)
                .SetEase(Ease.OutBack)
                .OnComplete(() =>
                {
                    textTransform.DOScale(1f, 0.1f).SetEase(Ease.InQuad);
                });
        }

        private void UpdateUI()
        {
            if (costText is not null)
            {
                costText.text = int.Parse(_currentCost.ToString()).ToString();
                PlayCostTextPopAnim();
            }
        }
        
        public int GetCurrentCost() => _currentCost;
    }
}
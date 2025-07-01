using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _00._Work._03._Scripts.Combat.Cost
{
    public class CostManager : MonoBehaviour
    {
        [Header("Cost Settings")] 
        public int maxCost = 10;
        public float increaseInterval = 1f;
        
        private int _currentCost;
        private float _timer;
        
        [Header("UI")]
        public List<Slider> costSlider;
        public TextMeshProUGUI costText;

        private void Start()
        {
            _currentCost = 0;
            _timer = 0;

            foreach (var slot in costSlider)
            {
                slot.value = 0;
            }
            
            UpdateUI();
        }

        private void Update()
        {
            if (_currentCost >= maxCost) return;

            _timer += Time.deltaTime;
            float progress = _timer / increaseInterval;

            // 현재 충전 중인 칸은 _currentCost 기준
            costSlider[_currentCost].value = Mathf.Clamp01(progress);

            if (_timer >= increaseInterval)
            {
                _timer = 0f;

                // 충전 완료된 칸에 애니메이션
                costSlider[_currentCost].value = 1f;
                PlayingSliderAnim(costSlider[_currentCost].transform);

                // 코스트 수치 증가
                AddCost(1);
                UpdateUI();
            }
        }

        public bool UseCost(int amount)
        {
            if (_currentCost < amount) return false;

            // 차감
            for (int i = 0; i < amount; i++)
            {
                _currentCost--;
                costSlider[_currentCost -1].value = 0f;
            }

            _timer = 0f;
            return true;
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

        private void UpdateUI()
        {
            if (costText is not null)
            {
                costText.text = int.Parse(_currentCost.ToString()).ToString();
            }
        }
        
        public int GetCurrentCost() => _currentCost;
    }
}
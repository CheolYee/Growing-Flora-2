using _00._Work._03._Scripts.Combat.Cost;
using _00._Work._03._Scripts.Combat.Skills;
using _00._Work._03._Scripts.Managers;
using UnityEngine;

namespace _00._Work._03._Scripts.SO.Skill
{
    public class SkillController : MonoBehaviour
    {
        [SerializeField] private float angleOffset;
        
        [SerializeField] private Transform skillCasterTransform;
        [SerializeField] private Transform skillTargetTransform;
        private int _currentSkillIndex = -1;
        private bool _isTargeting;
        
        public SkillTargetIndicator targetIndicator;
        public GameObject darkenOverlay;
        public float slowTimeScale = 0.3f;
        private Camera _camera;
        
        private Collider2D _currentTarget;
        private Material _originalMat;
        private Material _highlightMat;

        private void Start()
        {
            _camera = Camera.main;
        }

        public void Update()
        {
            if (!_isTargeting)
            {
                for (int i = 0; i < SkillUIManager.Instance.equippedSkills.Count; i++)
                {
                    if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                    {
                        SelectSkill(i);
                    }
                }
            }
            else
            {
                UpdateTargeting();
                
                if (Input.GetMouseButtonDown(0))
                {
                    ConfirmSkillCast();
                }

                if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
                {
                    CancelSkill();
                }
            }
        }

        private void CancelSkill()
        {
            Debug.Log("CancelSkill");
            ExitTargeting();
        }

        private void SelectSkill(int index)
        {
            if (SkillUIManager.Instance.equippedSkills[index] is null ||
                SkillUIManager.Instance.equippedSkills[index].cost > CostManager.Instance.GetCurrentCost()) return;
            _currentSkillIndex = index;
            _isTargeting = true;
            Time.timeScale = slowTimeScale;
            darkenOverlay.SetActive(true);
            
            SkillSo selectedSkill = SkillUIManager.Instance.equippedSkills[_currentSkillIndex];

            //선택된 스킬의 타입 및 범위 크기 전달
            targetIndicator.SetType(selectedSkill.rangeType, selectedSkill.lengthRadius, selectedSkill.widthRange);
            
        }

        private void UpdateTargeting()
        {
            Vector3 mouseWorldPos = _camera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;

            Vector3 casterPos = skillCasterTransform.position;
            SkillSo skill = SkillUIManager.Instance.equippedSkills[_currentSkillIndex];

            switch (skill.rangeType)
            {
                case SkillRangeType.BoxArea:
                    targetIndicator.transform.position = new Vector3(mouseWorldPos.x, mouseWorldPos.y, 0);
                    targetIndicator.transform.rotation = Quaternion.identity;
                    break;

                case SkillRangeType.ForwardLine:
                    Vector3 dir = (mouseWorldPos - casterPos).normalized;
                    float angleZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

                    // 시전자 기준 방향에 따라 위치 조정
                    targetIndicator.transform.position = casterPos;

                    // 시전자의 방향에 맞춰 회전
                    targetIndicator.transform.rotation = Quaternion.Euler(0, 0, angleZ);
                    break;
            }
        }

        private void ConfirmSkillCast()
        {
            SkillSo selectedSkill = SkillUIManager.Instance.equippedSkills[_currentSkillIndex];
            Vector3 mouseWorldPos = _camera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;

            Debug.Log($"스킬 발동: {selectedSkill.skillName}");

            // 코스트 차감
            CostManager.Instance.UseCost(selectedSkill.cost);

            if (selectedSkill.skillEffectPrefab != null)
            {
                string effectName = selectedSkill.skillEffectPrefab.name;
                IPoolable pooledObj = PoolManager.Instance.Pop(effectName);

                if (pooledObj is null)
                {
                    Debug.LogWarning($"풀링된 스킬 이펙트 '{effectName}' 를 찾을 수 없습니다.");
                    return;
                }

                var effect = pooledObj.GameObject.GetComponent<SkillEffectBase>();
                if (effect == null)
                {
                    Debug.LogWarning("SkillEffectBase 컴포넌트를 찾을 수 없음.");
                    return;
                }

                effect.Initialize(selectedSkill, skillCasterTransform);

                switch (selectedSkill.rangeType)
                {
                    case SkillRangeType.ForwardLine:
                        // 시전자 기준, 마우스 방향 벡터만 넘김
                        Vector3 dir = (mouseWorldPos - skillCasterTransform.position).normalized;
                        Vector3 targetPos = skillCasterTransform.position + dir * selectedSkill.lengthRadius;
                        effect.StartEffect(targetPos); // 방향만 넘김
                        break;

                    case SkillRangeType.BoxArea:
                        effect.StartEffect(mouseWorldPos); // 마우스 위치로 실행
                        break;
                    case SkillRangeType.TargetSingle:
                        Collider2D hit = Physics2D.OverlapPoint(mouseWorldPos, 5);
                        if (hit != null)
                        {
                            if (hit != _currentTarget)
                            {
                                ClearTargetHighlight();         // 기존 타겟 하이라이트 제거
                                _currentTarget = hit;
                                ShowTargetHighlight(hit);      // 새로운 타겟 하이라이트
                            }
                        }
                        else
                        {
                            ClearTargetHighlight();
                            _currentTarget = null;
                        }
                        break;

                    default:
                        Debug.LogWarning("알 수 없는 스킬 범위 타입입니다.");
                        break;
                }
            }

            ExitTargeting();
        }




        private void ExitTargeting()
        {
            targetIndicator.EndCasting();
            Time.timeScale = 1;
            darkenOverlay.SetActive(false);
            _isTargeting = false;
            _currentSkillIndex = -1;
        }
        
        private void ShowTargetHighlight(Collider2D target)
        {
            SpriteRenderer targetRenderer = target.GetComponent<SpriteRenderer>();
            if (targetRenderer != null)
            {
                _originalMat = targetRenderer.material;
                if (_highlightMat == null)
                {
                    _highlightMat = Resources.Load<Material>("HighlightOutline"); // Resources 폴더에 있어야 함
                }
                targetRenderer.material = _highlightMat;
            }
        }

        private void ClearTargetHighlight()
        {
            if (_currentTarget == null) return;

            SpriteRenderer targetRenderer = _currentTarget.GetComponent<SpriteRenderer>();
            if (targetRenderer != null && _originalMat != null)
            {
                targetRenderer.material = _originalMat;
            }
        }
    }
}
using UnityEngine;

namespace _00._Work._03._Scripts.SO.Skill
{
    public class SkillController : MonoBehaviour
    {
        [SerializeField] private Transform skillCasterTransform;
        private int _currentSkillIndex = -1;
        private bool _isTargeting;
        
        public SkillTargetIndicator targetIndicator;
        public GameObject darkenOverlay;
        public float slowTimeScale = 0.3f;
        private Camera _camera;

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
            if (SkillUIManager.Instance.equippedSkills[index] is null) return;
            _currentSkillIndex = index;
            _isTargeting = true;
            Time.timeScale = slowTimeScale;
            darkenOverlay.SetActive(true);
            
            SkillSo selectedSkill = SkillUIManager.Instance.equippedSkills[_currentSkillIndex];

            //선택된 스킬의 타입 및 범위 크기 전달
            targetIndicator.SetType(selectedSkill.rangeType, selectedSkill.rangeRadius);
            
        }

        private void UpdateTargeting()
        {
            Vector3 mouseScreenPos = Input.mousePosition;
            Vector3 mouseWorldPos = _camera.ScreenToWorldPoint(mouseScreenPos);
            mouseWorldPos.z = 0f;

            var skill = SkillUIManager.Instance.equippedSkills[_currentSkillIndex];

            switch (skill.rangeType)
            {
                case SkillRangeType.CircleArea:
                    // 마우스를 따라다니는 구조
                    targetIndicator.transform.position = mouseWorldPos;
                    targetIndicator.transform.rotation = Quaternion.identity;
                    break;

                case SkillRangeType.Self:
                case SkillRangeType.ForwardLine:
                case SkillRangeType.ConeArea:
                    // 캐릭터 기준 고정 위치
                    Vector3 casterPos = skillCasterTransform.position;
                    targetIndicator.transform.position = casterPos;

                    // 마우스 방향으로 회전
                    Vector3 dir = (mouseWorldPos - casterPos).normalized;
                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    targetIndicator.transform.rotation = Quaternion.Euler(0, 0, angle);
                    break;
            }
        }

        private void ConfirmSkillCast()
        {
            Vector3 targetPos = targetIndicator.transform.position;
            SkillSo selectedSkill = SkillUIManager.Instance.equippedSkills[_currentSkillIndex];
            
            Debug.Log($"스킬 발동: {selectedSkill.skillName}");

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
    }
}
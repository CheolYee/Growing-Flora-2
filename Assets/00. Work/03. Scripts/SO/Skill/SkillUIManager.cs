using System.Collections.Generic;
using _00._Work._02._Scripts.Manager;
using UnityEngine;

namespace _00._Work._03._Scripts.SO.Skill
{
    public class SkillUIManager : MonoSingleton<SkillUIManager>
    {
        [Header("Skill Settings")] 
        public List<SkillSo> equippedSkills;
        
        [Header("UI Elements")]
        public Transform skillPanelParent;
        public GameObject skillButtonPrefab;

        private void Start()
        {
            CreateSkillButton();
        }

        private void CreateSkillButton()
        {
            foreach (SkillSo skill in equippedSkills) //현재 편성된 캐릭터의 수만큼 스킬 데이터를 뽑아 그 수만큼 버튼 생성
            {
                GameObject skillButton = Instantiate(skillButtonPrefab, skillPanelParent); //버튼 생성
                SkillButton skillBtn = skillButton.GetComponent<SkillButton>(); //스킬 버튼 프리팹에서 컴포넌트 가져오기
                skillBtn.Initialize(skill); //스킬 버튼 초기화
            }
        }
    }
}
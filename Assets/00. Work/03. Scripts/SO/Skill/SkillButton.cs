using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _00._Work._03._Scripts.SO.Skill
{
    public class SkillButton : MonoBehaviour
    {
        [Header("References")] 
        public Image skillIcon;
        public TextMeshProUGUI costText;

        private SkillSo _skillData;

        public void Initialize(SkillSo data)
        {
            _skillData = data;

            if (skillIcon != null)
            {
                skillIcon.sprite = _skillData.skillIcon;
            }
            
            if (costText != null)
                costText.text = _skillData.cost.ToString();
        }

        public SkillSo GetSkillData()
        {
            return _skillData;
        }
    }
}
using UnityEngine;

namespace _00._Work._03._Scripts.SO.Skill
{
    public enum SkillType
    {
        None = 0,
        Single = 1,
        Area = 2
    }

    public enum SkillRangeType
    {
        None = 0,
        ForwardLine = 1,
        CircleArea = 2,
        ConeArea = 3,
        Self = 4
    }
    
    [CreateAssetMenu(fileName = "NewSkill", menuName = "SO/Skill Data")]
    public class SkillSo : ScriptableObject
    {
        [Header("Base Information")] 
        public string skillID; //스킬 아이디값
        public string skillName; //스킬 이름
        [TextArea]
        public string description; //설명
        
        [Header("Cost & Cooldown")]
        public int cost; //사용 코스트
        public float cooldown; //쿨타임

        [Header("Casting")] 
        public float castTime; //시전 시간
        public float preDelay; //선 딜레이
        public float postDelay; //후 딜레이
        
        [Header("Target & Range")]
        public SkillType skillType; //스킬 타입
        public SkillRangeType rangeType; //범위 타입
        public float rangeRadius; //범위 반지름, Forward는 길이로 사용
        public float rangeAngle; //Cone 타입용
        
        [Header("Skill Effects")]
        public GameObject skillEffectPrefab; //스킬 이펙트 프리팹
        public AudioClip skillSfx; //스킬 
        
        [Header("UI Elements")]
        public Sprite skillIcon;
    }
}

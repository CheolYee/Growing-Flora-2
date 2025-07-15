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
        TargetSingle = 1,
        ForwardLine = 2,
        BoxArea = 3,
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
        public int damage; //데미지

        [Header("Casting")] 
        public float castTime; //시전 시간
        public float preDelay; //선 딜레이
        public float activeDuration; //동작 시간 (움직임)
        public float postDelay; //후 딜레이
        
        [Header("Animation Names")]
        public string castAnimName;   // 준비 모션
        public string idleAnimName;   // 대기 모션
        public string fallAnimName;   // 낙하 모션
        public string activeAnimName; // 실행 모션
        public string endAnimName;    // 종료 모션
        
        [Header("Target & Range")]
        public SkillType skillType; //스킬 타입
        public SkillRangeType rangeType; //범위 타입
        public float lengthRadius; //세로 길이
        public float widthRange; //가로 길이
        
        [Header("Drop Settings")]
        public bool useDropEffect;         // 떨어지는 스킬인가?
        public float dropDuration = 0.5f;          // 떨어지는 데 걸리는 시간
        public Vector3 dropStartOffset = new(0, 5, 0); // 시전자 기준 떨어지는 시작 위치 오프셋
        
        [Header("Skill Effects")]
        public GameObject skillEffectPrefab; //스킬 이펙트 프리팹
        public AudioClip skillSfx; //스킬 
        
        [Header("UI Elements")]
        public Sprite skillIcon;
    }
}

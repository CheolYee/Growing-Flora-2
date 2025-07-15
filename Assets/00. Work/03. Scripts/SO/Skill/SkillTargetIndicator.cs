using UnityEngine;

namespace _00._Work._03._Scripts.SO.Skill
{
    public class SkillTargetIndicator : MonoBehaviour
    {
        public GameObject boxVisual;
        public GameObject lineVisual;
        public GameObject targetVisual;

        public void SetType(SkillRangeType skillType, float length, float width)
        {
            boxVisual.SetActive(false);
            lineVisual.SetActive(false);
            targetVisual.SetActive(false);

            switch (skillType)
            {
                case SkillRangeType.None:
                    break;

                case SkillRangeType.TargetSingle:
                    targetVisual?.SetActive(true);
                    if (targetVisual != null) targetVisual.transform.localScale = Vector3.one * 1f; // 크기 고정 또는 커스터마이즈
                    break;

                case SkillRangeType.ForwardLine:
                    lineVisual.SetActive(true);
                    lineVisual.transform.localScale = new Vector3(width, length, 1f);
                    break;

                case SkillRangeType.BoxArea:
                    boxVisual?.SetActive(true);
                    if (boxVisual != null) boxVisual.transform.localScale = new Vector3(width, length, 1f);
                    break;
            }
        }

        public void EndCasting()
        {
            boxVisual.SetActive(false);
            lineVisual.SetActive(false);
            targetVisual.SetActive(false);
        }
    }
}
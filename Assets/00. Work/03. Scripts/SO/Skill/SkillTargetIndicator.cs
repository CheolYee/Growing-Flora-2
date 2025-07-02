using UnityEngine;

namespace _00._Work._03._Scripts.SO.Skill
{
    public class SkillTargetIndicator : MonoBehaviour
    {
        public GameObject circleVisual;
        public GameObject lineVisual;
        public GameObject coneVisual;

        public void SetType(SkillRangeType skillType, float size)
        {
            circleVisual.SetActive(false);
            lineVisual.SetActive(false);
            coneVisual.SetActive(false);

            switch (skillType)
            {
                case SkillRangeType.None:
                    break;
                case SkillRangeType.CircleArea:
                    circleVisual.SetActive(true);
                    circleVisual.transform.localScale = Vector3.one * size;
                    break;
                case SkillRangeType.ForwardLine:
                    lineVisual.SetActive(true);
                    lineVisual.transform.localScale = new Vector3(size, 1f, 1f);
                    break;
                case SkillRangeType.ConeArea:
                    coneVisual.SetActive(true);
                    coneVisual.transform.localScale = Vector3.one * size;
                    break;
            }
        }

        public void EndCasting()
        {
            circleVisual.SetActive(false);
            lineVisual.SetActive(false);
            coneVisual.SetActive(false);
        }
    }
}
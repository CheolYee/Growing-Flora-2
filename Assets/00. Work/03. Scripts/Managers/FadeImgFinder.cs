using UnityEngine;
using UnityEngine.UI;

namespace _00._Work._03._Scripts.Managers
{
    public class FadeImgFinder : MonoBehaviour
    {
        private void Awake()
        {
            FadeManager.Instance.fadeImage = gameObject.GetComponent<Image>();
            FadeManager.Instance.FadeOut();
        }
    }
}
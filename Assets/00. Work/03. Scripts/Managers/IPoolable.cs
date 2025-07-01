using UnityEngine;

namespace _00._Work._03._Scripts.Managers
{
    public interface IPoolable
    {
        public string ItemName { get; }
        public GameObject GameObject { get; }
        public void ResetItem();
    }
}
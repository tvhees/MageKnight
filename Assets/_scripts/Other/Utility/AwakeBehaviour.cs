using UnityEngine;
using UnityEngine.Events;

namespace Other.Utility
{
    [System.Serializable]
    public class AwakeEvent : UnityEvent { }

    public class AwakeBehaviour : MonoBehaviour
    {
        public AwakeEvent awakeEvent = new AwakeEvent();

        private void Awake()
        {
            awakeEvent.Invoke();
        }
    }
}
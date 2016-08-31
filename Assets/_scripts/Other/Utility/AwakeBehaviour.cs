using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace Other.Utility
{
    [System.Serializable]
    public class AwakeEvent : UnityEvent { }

    public class AwakeBehaviour: MonoBehaviour 
	{
        public AwakeEvent awakeEvent = new AwakeEvent();

        void Awake()
        {
            awakeEvent.Invoke();
        }

	}
}
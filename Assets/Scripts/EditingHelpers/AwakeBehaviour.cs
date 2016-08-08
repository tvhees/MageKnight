using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
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
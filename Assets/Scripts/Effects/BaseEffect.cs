using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BoardGame
{
	namespace Effect
    {
		public abstract class BaseEffect : MonoBehaviour 
		{
            public int intValue;

            public abstract void UseEffect();
		}
	}
}
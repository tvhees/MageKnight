using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System;

namespace BoardGame
{
	namespace Effect
    {
		public class CombineEffects : BaseEffect 
		{

            private List<UnityAction> combinedEffects;

            public void AddEffects(List<UnityAction> combinedEffects)
            {
                this.combinedEffects = combinedEffects;    
            }

            public override void UseEffect()
            {
                for (int i = 0; i < combinedEffects.Count; i++) combinedEffects[i].Invoke();
            }
        }
	}
}
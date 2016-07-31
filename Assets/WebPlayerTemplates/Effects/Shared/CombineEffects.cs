using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Boardgame
{
	namespace Effect
    {
		public class CombineEffects : MonoBehaviour, BaseEffect 
		{

            private List<UnityAction> combinedEffects;

            private EffectData effectData;

            public void AddEffects(List<UnityAction> combinedEffects)
            {
                this.combinedEffects = combinedEffects;    
            }

            public void Execute()
            {
                for (int i = 0; i < combinedEffects.Count; i++) combinedEffects[i].Invoke();
            }

            public EffectData GetEffectData()
            {
                if (effectData == null)
                    effectData = new EffectData();

                return effectData;
            }
        }
	}
}
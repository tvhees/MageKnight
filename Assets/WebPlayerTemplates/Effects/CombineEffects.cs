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
		public class CombineEffects : BaseEffect 
		{

            private List<UnityAction<Rulesets.Ruleset>> combinedEffects;

            public void AddEffects(List<UnityAction<Rulesets.Ruleset>> combinedEffects)
            {
                this.combinedEffects = combinedEffects;    
            }

            public override void UseEffect(Rulesets.Ruleset rules)
            {
                for (int i = 0; i < combinedEffects.Count; i++) combinedEffects[i].Invoke(Game.GetRules());
            }
        }
	}
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Boardgame
{
	namespace Effect
    {
		public class Heal : BaseEffect 
		{
            public override void UseEffect(Rulesets.Ruleset rules)
            {
                Game.GetRules().AddHealing(effectData);
                GetComponent<CleanupMethod>().Success();
            }
        }
	}
}
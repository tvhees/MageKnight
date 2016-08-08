using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Boardgame
{
	namespace Effect
    {
		public class HealWound : BaseEffect 
		{
            public override void UseEffect(Rulesets.Ruleset rules)
            {
                if (Rules.Healing.Instance.totalPaid < intValue) return;

                GetComponent<CleanupMethod>().ThrowAway(GetComponentInParent<Card.Object>());
                Rules.Healing.Instance.AddHealing(-intValue);
            }
        }
	}
}
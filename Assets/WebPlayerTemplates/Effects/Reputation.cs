using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Boardgame
{
	namespace Effect
    {
		public class Reputation : BaseEffect 
		{
            public override void UseEffect(Rulesets.Ruleset rules)
            {
                GetComponentInParent<Card.Object>().owningPlayer.AddReputation(intValue);

                GetComponent<CleanupMethod>().Success();
            }
        }
	}
}
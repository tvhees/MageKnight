using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Boardgame
{
	namespace Effect
    {
		public class TakeWound : BaseEffect 
		{
            public override void UseEffect(Rulesets.Ruleset rules)
            {
                GetComponentInParent<Card.Object>().owningPlayer.TakeWound(Card.Object.Location.hand);


                GetComponent<CleanupMethod>().Success();
            }
        }
	}
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Boardgame
{
	namespace Effect
    {
		public class PlayFromOffer : BaseEffect 
		{
            public override void UseEffect(Rulesets.Ruleset rules)
            {
                GetComponent<CleanupMethod>().Success();
            }
        }
	}
}
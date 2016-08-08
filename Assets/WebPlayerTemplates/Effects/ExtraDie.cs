using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Boardgame
{
	namespace Effect
    {
		public class ExtraDie : BaseEffect 
		{
            public override void UseEffect(Rulesets.Ruleset rules)
            {
                // Allow an extra mana die to use this turn

                GetComponent<CleanupMethod>().Success();
            }
        }
	}
}
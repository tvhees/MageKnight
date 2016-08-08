using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Boardgame
{
	namespace Effect
    {
		public class ManaToken : BaseEffect 
		{
            public override void UseEffect(Rulesets.Ruleset rules)
            {
                // Gain a mana token

                GetComponent<CleanupMethod>().Success();
            }
        }
	}
}
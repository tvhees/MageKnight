using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Boardgame
{
	namespace Effect
    {
		public class Concentration : BaseEffect 
		{
            public override void UseEffect(Rulesets.Ruleset rules)
            {
                // Get mana tokens or power a card

                GetComponent<CleanupMethod>().Success();
            }
        }
	}
}
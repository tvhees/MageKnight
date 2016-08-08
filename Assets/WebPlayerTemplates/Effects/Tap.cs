using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Boardgame
{
	namespace Effect
    {
		public class Tap : BaseEffect 
		{
            public override void UseEffect(Rulesets.Ruleset rules)
            {
                rules.AddMovement(effectData);
                GetComponent<CleanupMethod>().Success();
            }
        }
	}
}
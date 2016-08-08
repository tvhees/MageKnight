using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Boardgame
{
	namespace Effect
    {
		public class RerollDie : BaseEffect 
		{
            public override void UseEffect(Rulesets.Ruleset rules)
            {
                GetComponent<CleanupMethod>().Success();
            }
        }
	}
}
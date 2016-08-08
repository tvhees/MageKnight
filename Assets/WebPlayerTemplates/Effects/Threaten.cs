using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Boardgame
{
	namespace Effect
    {
		public class Threaten : BaseEffect 
		{
            public override void UseEffect(Rulesets.Ruleset rules)
            {
                rules.AddInfluence(effectData);
                //rules.AddReputation(effectData);

                GetComponent<CleanupMethod>().Success();
            }
        }
	}
}
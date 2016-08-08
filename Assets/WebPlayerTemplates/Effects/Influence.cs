using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Boardgame
{
	namespace Effect
    {
		public class Influence : BaseEffect 
		{
            public override void UseEffect(Rulesets.Ruleset rules)
            {
                
                Rules.Influence.Instance.AddInfluence(intValue);                        
                GetComponent<CleanupMethod>().Success();
            }
        }
	}
}
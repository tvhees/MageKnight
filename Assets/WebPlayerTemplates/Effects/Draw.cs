using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Boardgame
{
	namespace Effect
    {
		public class Draw : BaseEffect 
		{
            public override void UseEffect(Rulesets.Ruleset rules)
            {
                GetComponent<CardButton>().player.DrawCards(intValue);
                GetComponent<CleanupMethod>().Success();
            }
        }
	}
}
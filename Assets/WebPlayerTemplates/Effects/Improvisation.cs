using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Boardgame
{
	namespace Effect
    {
		public class Improvisation : BaseEffect 
		{
            public override void UseEffect(Rulesets.Ruleset rules)
            {
                // Discard a card, choose a basic effect
                rules.AddMovement(effectData);

                GetComponent<CleanupMethod>().Success();
            }
        }
	}
}
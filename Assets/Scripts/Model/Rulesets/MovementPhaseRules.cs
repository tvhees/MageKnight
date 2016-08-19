using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Rulesets
{
public class MovementPhaseRules : RulesetExtension
	{
        public MovementPhaseRules(Ruleset extendedRuleset)
        {
            this.extendedRuleset = extendedRuleset;
        }
    }
}
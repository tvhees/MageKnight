using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Rulesets
{
public class CombatPhaseRules : RulesetExtension
	{
        public CombatPhaseRules(Ruleset extendedRuleset)
        {
            this.extendedRuleset = extendedRuleset;
        }
	}
}
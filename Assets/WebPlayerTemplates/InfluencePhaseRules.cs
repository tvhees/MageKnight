using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Rulesets
{
public class InfluencePhaseRules : RulesetExtension
	{
        public InfluencePhaseRules(Ruleset extendedRuleset)
        {
            this.extendedRuleset = extendedRuleset;
        }
	}
}
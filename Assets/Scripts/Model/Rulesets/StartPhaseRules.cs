using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Rulesets
{
public class StartPhaseRules : RulesetExtension
	{
        public StartPhaseRules (Ruleset extendedRuleset)
        {
            this.extendedRuleset = extendedRuleset;
            Players.currentPlayer.NewVariables();
        }
	}
}
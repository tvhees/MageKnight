using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Rulesets
{
    public class FullConquestRules : RulesetExtension
    {
        public FullConquestRules(Ruleset extendedRuleset)
        {
            this.extendedRuleset = extendedRuleset;
        }

    }
}
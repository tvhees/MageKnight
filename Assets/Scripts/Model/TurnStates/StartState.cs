using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Boardgame.Model
{
	public class StartState : TurnState
	{
        public override void BeginState()
        {
            Rulesets.MovementCosts.Reset(true);
        }

        public override Rulesets.Ruleset GetRuleset(Rulesets.Ruleset baseRuleset)
        {
            return new Rulesets.StartPhaseRules(baseRuleset);
        }

        public override Command[] GetCleanupCommands()
        {
            return new Command[0] { };
        }
    }
}
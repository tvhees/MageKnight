using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Model
{
	public class InfluenceState : TurnState
	{
        public override void EndCurrentState()
        {
            turn.SetState(turn.GetEndState());
        }

        public override Rulesets.Ruleset GetRuleset(Rulesets.Ruleset baseRuleset)
        {
            return new Rulesets.InfluencePhaseRules(baseRuleset);
        }

        public override void CleanUpState()
        {
            // Clean up this state
        }
    }
}
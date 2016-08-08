using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Model
{
	public class EndState : TurnState
	{
        public override void EndCurrentState()
        {
            turn.EndTurn();
        }

        public override Rulesets.Ruleset GetRuleset(Rulesets.Ruleset baseRuleset)
        {
            return new Rulesets.MovementPhaseRules(baseRuleset);
        }

        public override void CleanUpState()
        {
            // Clean up this state
        }
    }
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Model
{
	public class MovementState : TurnState
	{
        public override void StartState()
        {
            pathDrawer.StartPath();
        }

        public override void EndCurrentState()
        {
            turn.SetState(turn.GetInfluenceState());
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
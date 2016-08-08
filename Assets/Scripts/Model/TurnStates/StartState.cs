using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Boardgame.Model
{
	public class StartState : TurnState
	{
        public override void EndCurrentState()
        {
            turn.SetState(turn.GetMovementState());
        }

        public override Rulesets.Ruleset GetRuleset(Rulesets.Ruleset baseRuleset)
        {
            return new Rulesets.StartPhaseRules(baseRuleset);
        }

        public override void CleanUpState()
        {
            // Clean up this state
        }
    }
}
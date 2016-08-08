using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Boardgame.Model
{
	public class CombatState : TurnState
	{
        public override void EndCurrentState()
        {
            turn.SetState(turn.GetEndState());
        }

        public override Rulesets.Ruleset GetRuleset(Rulesets.Ruleset baseRuleset)
        {
            return new Rulesets.CombatPhaseRules(baseRuleset);
        }

        public override void CleanUpState()
        {
            // Clean up this state
        }
    }
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Boardgame.Game
{
	public class CombatState : TurnState
	{
        private Turn turn;

        public CombatState(Turn turn)
        {
            this.turn = turn;
        }

        public override void EndCurrentState()
        {
            turn.SetState(turn.GetEndState());
        }

        public override Rulesets.Ruleset GetRuleset()
        {
            return new Rulesets.MovementPhaseExtension(new Rulesets.BaseRuleset());
        }
    }
}
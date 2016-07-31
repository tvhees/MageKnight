using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Boardgame.Game
{
	public class StartState : TurnState
	{
        private Turn turn;

        public StartState(Turn turn)
        {
            this.turn = turn;
        }

        public override void EndCurrentState()
        {
            turn.SetState(turn.GetMovementState());
        }

        public override Rulesets.Ruleset GetRuleset()
        {
            return new Rulesets.MovementPhaseExtension(new Rulesets.BaseRuleset());
        }
    }
}
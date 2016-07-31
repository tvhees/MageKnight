using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Game
{
	public class EndState : TurnState
	{
        private Turn turn;

        public EndState(Turn turn)
        {
            this.turn = turn;
        }

        public override void EndCurrentState()
        {
            turn.EndTurn();
        }

        public override Rulesets.Ruleset GetRuleset()
        {
            return new Rulesets.MovementPhaseExtension(new Rulesets.BaseRuleset());
        }
    }
}
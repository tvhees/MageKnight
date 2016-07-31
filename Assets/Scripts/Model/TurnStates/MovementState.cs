using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Game
{
	public class MovementState : TurnState
	{
        private Turn turn;

        public MovementState(Turn turn)
        {
            this.turn = turn;
        }

        public override void EndCurrentState()
        {
            turn.SetState(turn.GetInfluenceState());
        }

        public override Rulesets.Ruleset GetRuleset()
        {
            return new Rulesets.MovementPhaseExtension(new Rulesets.BaseRuleset());
        }
    }
}
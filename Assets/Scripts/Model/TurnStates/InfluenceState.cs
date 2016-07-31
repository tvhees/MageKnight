using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Game
{
	public class InfluenceState : TurnState
	{
        private Turn turn;

        public InfluenceState(Turn turn)
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
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Game
{
	public class Rules : Rulesets.RulesetExtension, Observer<Turn>
    {
        private Turn turn;
        private Rulesets.Ruleset ruleSet;

        void Awake()
        {
            turn = new Turn();
            turn.RegisterObserver(this);
            turn.SetState(turn.GetStartState());
        }

        public void UpdateObserver(Turn turn)
        {
            ruleSet = turn.GetRuleSet();

        }

        void UpdateDelegateReferences()
        {

        }
	}
}
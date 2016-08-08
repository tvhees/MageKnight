using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Model
{
	public abstract class TurnState : MonoBehaviour
	{
        protected Turn turn;
        protected PathDrawer pathDrawer;

        void Awake()
        {
            turn = FindObjectOfType<Turn>();
            pathDrawer = FindObjectOfType<PathDrawer>();
        }

        public virtual void StartState()
        {

        }

        public abstract void EndCurrentState();

        public abstract Rulesets.Ruleset GetRuleset(Rulesets.Ruleset baseRuleset);

        public abstract void CleanUpState();
	}
}
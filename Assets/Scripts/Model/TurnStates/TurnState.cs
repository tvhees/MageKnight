using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Game
{
	public abstract class TurnState : MonoBehaviour 
	{
        public abstract void EndCurrentState();

        public abstract Rulesets.Ruleset GetRuleset();

        public void CleanUpState()
        {
            Debug.Log("Cleaning up effects from this state");    
        }
	}
}
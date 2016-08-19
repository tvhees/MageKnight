using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Model
{
	public abstract class TurnState : MonoBehaviour
	{
        public virtual void BeginState()
        {

        }

        public abstract Rulesets.Ruleset GetRuleset(Rulesets.Ruleset baseRuleset);

        public abstract Command[] GetCleanupCommands();
	}
}
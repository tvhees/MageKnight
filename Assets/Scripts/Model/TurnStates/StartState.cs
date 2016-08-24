using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Boardgame.Model
{
	public class StartState : TurnState
	{
        public override void BeginState()
        {
            Rulesets.MovementCosts.Reset(true);
        }

        public override Command[] GetCleanupCommands()
        {
            return new Command[0] { };
        }
    }
}
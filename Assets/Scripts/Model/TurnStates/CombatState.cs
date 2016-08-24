using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Boardgame.Model
{
	public class CombatState : TurnState
	{
        public override Command[] GetCleanupCommands()
        {
            return new Command[0] { };
        }
    }
}
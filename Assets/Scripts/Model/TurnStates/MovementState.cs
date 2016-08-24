using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Model
{
	public class MovementState : TurnState
	{
        public override Command[] GetCleanupCommands()
        {
            return new Command[0] { };
        }
    }
}
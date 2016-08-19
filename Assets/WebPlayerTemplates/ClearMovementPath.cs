using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Boardgame
{
    public class ClearMovementPath : Command
    {
        Player.MovementPath movementPath;

        public ClearMovementPath()
        {
            movementPath = Main.players.currentPlayer.movementPath;
            for (int i = movementPath.count - 1; i > 0; i--)
            {
                requirements.Add(new RemoveTileFromMovementPath(i, movementPath));
            }
        }

        protected override CommandResult ExecuteThisCommand()
        {
            return CommandResult.success;
        }

        protected override void UndoThisCommand()
        {
            
        }
    }
}
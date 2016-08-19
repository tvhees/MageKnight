using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Boardgame
{
    public class RemoveTileFromMovementPath : Command
    {
        Player.MovementPath movementPath;
        GameObject tile;
        int index;

        public RemoveTileFromMovementPath(int index, Player.MovementPath movementPath)
        {
            this.movementPath = movementPath;
            this.index = index;
            tile = movementPath.tileAtIndexInPath(index);

            Debug.Log(tile.name);
        }

        protected override CommandResult ExecuteThisCommand()
        {
            movementPath.RemoveDestinationTile(tile);
            Main.pathDrawer.RemoveSegmentAtIndex(index);

            return CommandResult.success;
        }

        protected override void UndoThisCommand()
        {
            Vector3 origin = movementPath.lastPositionInPath;
            movementPath.AddDestinationTile(tile);
            Main.pathDrawer.AddSegmentToPath(origin, tile);
        }
    }
}
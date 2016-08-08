using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
    public class AddTileToMovementPath: Command 
	{
        PlayerImpl player;
        GameObject tile;

        public AddTileToMovementPath(PlayerImpl player, GameObject tile)
        {
            this.player = player;
            this.tile = tile;
        }

        public override bool Execute()
        {
            if (player.movementPath.CanMoveToTile(tile))
            {
                Debug.Log("Adding hex to movement path");

                Vector3 origin = player.movementPath.lastPositionInPath;
                player.movementPath.AddDestinationTile(tile);
                Main.Instance.pathDrawer.AddSegmentToPath(origin, tile.transform.position);

                return true;
            }
            else
                return false;
        }

        public override void Undo()
        {
            Debug.Log("Undo: Removing hex from movement path");
            player.movementPath.RemoveLastTile();
            Main.Instance.pathDrawer.RemoveLastSegment();
        }
	}
}
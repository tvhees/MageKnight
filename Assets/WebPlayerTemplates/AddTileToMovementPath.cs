using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
    public class AddTileToMovementPath: Command 
	{
        PlayerImpl player;
        GameObject tile;

        public AddTileToMovementPath(GameObject tile)
        {
            Model.Turn turn = Main.turn;
            player = Main.players.currentPlayer;
            this.tile = tile;

            if(turn.currentState != turn.GetMovementState())
                requirements.Add(new ChangeTurnState(turn, turn.GetMovementState()));
        }

        protected override CommandResult ExecuteThisCommand()
        {
            if (player.movementPath.CanMoveToTile(tile))
            {
                Vector3 origin = player.movementPath.lastPositionInPath;

                player.movementPath.AddDestinationTile(tile);
                Main.pathDrawer.AddSegmentToPath(origin, tile);

                return CommandResult.success;
            }
            else
            {
                UndoCompletedRequirements();
                return new CommandResult(new InfluenceLocals(tile));
            }
        }

        protected override void UndoThisCommand()
        {
            player.movementPath.RemoveDestinationTile(tile);
            Main.pathDrawer.RemoveLastSegment();
        }
	}
}
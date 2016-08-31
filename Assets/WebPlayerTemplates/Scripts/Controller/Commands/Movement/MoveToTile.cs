using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Boardgame
{
    public class MoveToTile : Command
    {
        private PlayerImpl player;
        private GameObject oldTile;
        private GameObject newTile;
        private int cost;

        public MoveToTile(GameObject newTile)
        {
            Model.Turn turn = Main.turn;
            player = Main.players.currentPlayer;
            oldTile = player.tile;
            this.newTile = newTile;
            cost = newTile.GetComponent<Board.Terrain>().movementCost;

            if (turn.currentState != turn.GetMovementState())
                requirements.Add(new ChangeTurnState(turn.GetMovementState()));
        }

        protected override CommandResult ExecuteThisCommand()
        {
            if (player.CanMoveToTile(newTile))
            {
                player.MoveToTile(newTile);
                player.AddMovement(-cost);

                return CommandResult.success;
            }
            else
                return CommandResult.failure;
        }

        protected override void UndoThisCommand()
        {
            player.MoveToTile(oldTile);
            player.AddMovement(cost);
        }
    }
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Commands
{
    public class MoveToTile : Command
    {
        private PlayerControl player;
        private GameObject oldTile;
        private GameObject newTile;
        private int cost;

        public MoveToTile(GameObject newTile)
        {
            StateController stateController = GameController.singleton.stateController;
            player = GameController.singleton.currentPlayer;
            oldTile = player.model.currentTile;
            this.newTile = newTile;
            cost = newTile.GetComponent<Hex>().movementCost;

            if (stateController.gameState != stateController.movement)
                requirements.Add(new ChangeTurnState(stateController.movement));
        }

        protected override CommandResult ExecuteThisCommand()
        {
            if (player.CanMoveToTile(newTile))
            {
                player.ServerMoveToTile(newTile);
                player.ServerAddMovement(-cost);

                return CommandResult.success;
            }
            else
                return CommandResult.failure;
        }

        protected override void UndoThisCommand()
        {
            player.ServerMoveToTile(oldTile);
            player.ServerAddMovement(cost);
        }
    }
}
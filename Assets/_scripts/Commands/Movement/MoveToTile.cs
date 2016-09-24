using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Commands
{
    public class MoveToTile : Command
    {
        private PlayerControl player;
        private HexId oldHex;
        private HexId newHex;
        private int cost;

        public MoveToTile(HexId newHex)
        {
            StateController stateController = GameController.singleton.stateController;
            player = GameController.singleton.currentPlayer;
            oldHex = player.currentHex;
            this.newHex = newHex;
            cost = newHex.movementCost;

            if (stateController.gameState != stateController.movement)
                requirements.Add(new ChangeTurnState(stateController.movement));
        }

        protected override CommandResult ExecuteThisCommand()
        {
            if (player.CanMoveToHex(newHex))
            {
                player.ServerMoveToHex(newHex);
                player.ServerAddMovement(-cost);

                return CommandResult.success;
            }
            else
                return CommandResult.failure;
        }

        protected override void UndoThisCommand()
        {
            player.ServerMoveToHex(oldHex);
            player.ServerAddMovement(cost);
        }
    }
}
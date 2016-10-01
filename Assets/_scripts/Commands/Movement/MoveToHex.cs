using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Commands
{
    [CreateAssetMenu(menuName = "Command/Move To Hex")]
    public class MoveToHex : Command
    {
        private HexId originalHex;

        public override void SetInformation(GameData input)
        {
            base.SetInformation(input);
            StateController stateController = GameController.singleton.stateController;
        }

        protected override CommandResult ExecuteThisCommand()
        {
            PlayerControl player = gameData.player;
            if (player.CanMoveToHex(gameData.hexId))
            {
                originalHex = player.currentHex;
                player.OnHexChanged(gameData.hexId);
                player.ServerAddMovement(-gameData.hexId.movementCost);

                return CommandResult.success;
            }
            else
                return CommandResult.failure;
        }

        protected override void UndoThisCommand()
        {
            gameData.player.OnHexChanged(originalHex);
            gameData.player.ServerAddMovement(gameData.hexId.movementCost);
        }
    }
}
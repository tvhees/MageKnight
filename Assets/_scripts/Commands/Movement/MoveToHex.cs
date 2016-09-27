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
        private HexId targetHex;
        private int cost;

        public void SetInformation(HexId targetHex)
        {
            int i = 0;
            StateController stateController = GameController.singleton.stateController;
            this.targetHex = targetHex;
            cost = targetHex.movementCost;
            
            if (stateController.gameState != stateController.movement)
            {
                var changeState = ScriptableObject.CreateInstance<ChangeTurnState>();
                changeState.SetInformation(stateController.movement);
                requirements.Add(changeState);
            }
        }

        protected override CommandResult ExecuteThisCommand()
        {
            if (player.CanMoveToHex(targetHex))
            {
                originalHex = player.currentHex;
                player.OnHexChanged(targetHex);
                player.ServerAddMovement(-cost);

                return CommandResult.success;
            }
            else
                return CommandResult.failure;
        }

        protected override void UndoThisCommand()
        {
            player.OnHexChanged(originalHex);
            player.ServerAddMovement(cost);
        }
    }
}
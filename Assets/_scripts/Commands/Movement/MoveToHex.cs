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

        public override IEnumerator Routine(Action<GameConstants.Location> resolve, Action<Exception> reject)
        {
            yield return null;

            PlayerControl player = gameData.player;
            if (player.CanMoveToHex(gameData.hexId))
            {
                originalHex = player.CurrentHex;
                player.OnHexChanged(gameData.hexId);
                player.ServerAddMovement(-gameData.hexId.movementCost);

                resolve(GameConstants.Location.Play);
            }
            else
                reject(null);
        }

        protected override void UndoThisCommand()
        {
            gameData.player.OnHexChanged(originalHex);
            gameData.player.ServerAddMovement(gameData.hexId.movementCost);
        }
    }
}
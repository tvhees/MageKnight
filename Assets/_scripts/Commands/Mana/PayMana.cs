using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Commands
{
    [CreateAssetMenu(menuName = "Command/Pay Mana")]
    public class PayMana : Command
    {
        public GameConstants.ManaType colour;
        GameConstants.ManaType paidColour;
        ManaId paidId;
        Player playerModel;

        public override IEnumerator Routine(Action<CommandResult> resolve, Action<Exception> reject)
        {
            yield return null;
            playerModel = gameData.player.model;
            var success = true;
            // We can use gold mana instead of non-black but need to store this.
            if (playerModel.HasMana(colour))
                paidColour = colour;
            else if (playerModel.HasGold && colour != GameConstants.ManaType.Black)
                paidColour = GameConstants.ManaType.Gold;
            else
                success = false;

            if (success)
            {
                playerModel.AddMana(paidColour, true);
                paidId = GameController.singleton.PlayManaSource(paidColour);
            }

            resolve(new CommandResult(success));
        }

        public override void UndoThisCommand()
        {
            GameController.singleton.ServerReturnManaSource(paidId);
            playerModel.AddMana(paidColour);
        }
    }
}
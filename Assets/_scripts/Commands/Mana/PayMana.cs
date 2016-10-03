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
        private GameConstants.ManaType paidColour;
        private Player playerModel;

        protected override CommandResult ExecuteThisCommand()
        {
            playerModel = gameData.player.model;
            // We can use gold mana instead of non-black but need to record this.
            if (playerModel.HasMana(colour))
                paidColour = colour;
            else if (playerModel.HasGold && colour != GameConstants.ManaType.Black)
                paidColour = GameConstants.ManaType.Gold;
            else
                return CommandResult.failure;
            
            playerModel.AddMana(paidColour, subtract: true);
            return CommandResult.success;
        }

        protected override void UndoThisCommand()
        {
            playerModel.AddMana(paidColour);
        }
    }
}
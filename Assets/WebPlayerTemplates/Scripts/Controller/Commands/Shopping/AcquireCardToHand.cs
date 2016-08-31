using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Boardgame
{
    public class AcquireCardToHand : AcquireCard
    {
        public AcquireCardToHand(Cards.Acquirable card, Command acquisitionCost = null)
        {
            SetParameters(card, acquisitionCost);
        }

        protected override CommandResult ExecuteThisCommand()
        {
            Main.players.currentPlayer.belongings.GainCardToHand(card);

            return base.ExecuteThisCommand();
        }
    }
}
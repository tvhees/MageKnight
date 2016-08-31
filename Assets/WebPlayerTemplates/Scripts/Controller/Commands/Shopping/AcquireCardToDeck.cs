using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Boardgame
{
    public class AcquireCardToDeck : AcquireCard
    {
        public AcquireCardToDeck(Cards.Acquirable card, Command acquisitionCost = null)
        {
            SetParameters(card, acquisitionCost);
        }

        protected override CommandResult ExecuteThisCommand()
        {
            Main.players.currentPlayer.belongings.GainCardToDeck(card);

            return base.ExecuteThisCommand();
        }
    }
}
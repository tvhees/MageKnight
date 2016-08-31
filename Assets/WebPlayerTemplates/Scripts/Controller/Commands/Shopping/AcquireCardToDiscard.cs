﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Boardgame
{
    public class AcquireCardToDiscard : AcquireCard
    {
        public AcquireCardToDiscard(Cards.Acquirable card, Command acquisitionCost = null)
        {
            SetParameters(card, acquisitionCost);
        }

        protected override CommandResult ExecuteThisCommand()
        {
            Main.players.currentPlayer.belongings.GainCardToDiscard(card);

            return base.ExecuteThisCommand();
        }
    }
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
    public class AcquireUnit : AcquireCard 
	{
        public AcquireUnit(Cards.Acquirable card, Command acquisitionCost = null)
        {
            SetParameters(card, acquisitionCost);
        }

        protected override CommandResult ExecuteThisCommand()
        {
            Main.players.currentPlayer.belongings.GainUnit(card);

            return base.ExecuteThisCommand();
        }
    }
}
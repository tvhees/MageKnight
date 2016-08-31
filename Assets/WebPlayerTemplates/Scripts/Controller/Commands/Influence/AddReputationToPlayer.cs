using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
    public class AddReputationToPlayer: Command 
	{
        PlayerImpl player;
        int reputation;

        public AddReputationToPlayer(PlayerImpl player, int reputation)
        {
            this.player = player;
            this.reputation = reputation;
        }

        protected override CommandResult ExecuteThisCommand()
        {
            reputation = player.AddReputation(reputation);
            return new CommandResult(reputation != 0);
        }

        protected override void UndoThisCommand()
        {
            player.AddReputation(-reputation);
        }
    }
}
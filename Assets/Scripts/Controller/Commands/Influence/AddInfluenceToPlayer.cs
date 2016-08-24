using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
    public class AddInfluenceToPlayer: Command 
	{
        PlayerImpl player;
        int influence;

        public AddInfluenceToPlayer(PlayerImpl player, int influence)
        {
            this.player = player;
            this.influence = influence;
        }

        protected override CommandResult ExecuteThisCommand()
        {
            return new CommandResult(player.AddInfluence(influence));
        }

        protected override void UndoThisCommand()
        {
            player.AddInfluence(-influence);
        }
    }
}
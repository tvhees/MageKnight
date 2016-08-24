using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Boardgame
{
    public class AddHealingToPlayer : Command
    {
        PlayerImpl player;
        int healing;

        public AddHealingToPlayer(PlayerImpl player, int healing, int cost)
        {
            this.player = player;
            this.healing = healing;

            if (cost > 0)
                requirements.Add(new AddInfluenceToPlayer(player, -cost));
        }

        protected override CommandResult ExecuteThisCommand()
        {
            player.AddHealing(healing);

            return CommandResult.success;
        }

        protected override void UndoThisCommand()
        {
            player.AddHealing(-healing);
        }
    }
}
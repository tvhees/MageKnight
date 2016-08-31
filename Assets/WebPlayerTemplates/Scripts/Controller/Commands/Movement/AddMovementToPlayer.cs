using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Boardgame
{
    public class AddMovementToPlayer : Command
    {
        PlayerImpl player;
        int movement;

        public AddMovementToPlayer(PlayerImpl player, int movement)
        {
            this.player = player;
            this.movement = movement;
        }

        protected override CommandResult ExecuteThisCommand()
        {
            player.AddMovement(movement);

            return CommandResult.success;
        }

        protected override void UndoThisCommand()
        {
            player.AddMovement(-movement);
        }
    }
}
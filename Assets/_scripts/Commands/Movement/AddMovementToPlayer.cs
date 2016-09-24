using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Commands
{
    public class AddMovementToPlayer : Command
    {
        PlayerControl player;
        int movement;

        public AddMovementToPlayer(PlayerControl player, int movement)
        {
            this.player = player;
            this.movement = movement;
        }

        protected override CommandResult ExecuteThisCommand()
        {
            player.ServerAddMovement(movement);

            return CommandResult.success;
        }

        protected override void UndoThisCommand()
        {
            player.ServerAddMovement(-movement);
        }
    }
}
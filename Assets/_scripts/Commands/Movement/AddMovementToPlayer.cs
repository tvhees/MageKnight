using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Commands
{
    [CreateAssetMenu(menuName = "Command/Add Movement")]
    public class AddMovementToPlayer : Command
    {
        public int movement;

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
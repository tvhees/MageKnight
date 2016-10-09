using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Commands
{
    [CreateAssetMenu(menuName = "Command/Draw Cards")]
    public class DrawCards : Command
    {
        public int valueSize;

        public override void SetInformation(GameData input)
        {
            base.SetInformation(input);
        }

        protected override CommandResult ExecuteThisCommand()
        {
            if (gameData.player.model.CanDrawCards)
            {
                gameData.player.DrawCards(valueSize);
                return CommandResult.success;
            }

            return CommandResult.failure;
        }
    }
}
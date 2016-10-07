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
            bool succeeded = gameData.player.DrawCards(valueSize);
            return new CommandResult(succeeded, allowUndo: false);
        }

        protected override void UndoThisCommand()
        {
            
        }
    }
}
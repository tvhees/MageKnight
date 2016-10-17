using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Commands
{
    [CreateAssetMenu(menuName = "Command/Move Card")]
    public class MoveCard : Command
    {
        public GameConstants.Location fromLocation;
        public GameConstants.Location toLocation;
        CardId card;

        public override void SetInformation(GameData input)
        {
            base.SetInformation(input);
            card = input.cardId;
        }

        public override IEnumerator Routine(Action resolve, Action<Exception> reject)
        {
            gameData.player.ServerMoveCard(card, toLocation);

            yield return null;

            resolve();
        }

        public override IEnumerator Routine(Action<CommandResult> resolve, Action<Exception> reject)
        {
            gameData.player.ServerMoveCard(card, toLocation);

            yield return null;

            resolve(CommandResult.success);
        }

        protected override void UndoThisCommand()
        {
            gameData.player.ServerMoveCard(card, fromLocation);
        }
    }
}
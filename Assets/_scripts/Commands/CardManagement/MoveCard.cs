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
        private CardId card;

        public override void SetInformation(GameData input)
        {
            base.SetInformation(input);
            card = input.cardId;
        }

        public override IEnumerator Routine(Action<GameConstants.Location> resolve, Action<Exception> reject)
        {
            yield return null;
            resolve(toLocation);
        }

        protected override void UndoThisCommand()
        {
            gameData.player.ServerMoveCard(card, fromLocation);
        }
    }
}
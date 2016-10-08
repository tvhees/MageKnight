using UnityEngine;

namespace Commands
{
    [CreateAssetMenu(menuName = "Command/Move Card")]
    public class MoveCard : Command
    {
        public GameConstants.Collection fromCollection;
        public GameConstants.Collection toCollection;
        private CardId card;

        public override void SetInformation(GameData input)
        {
            base.SetInformation(input);
            card = input.cardId;
        }

        protected override CommandResult ExecuteThisCommand()
        {
            gameData.player.ServerMoveCard(card, toCollection);
            return CommandResult.success;
        }

        protected override void UndoThisCommand()
        {
            gameData.player.ServerMoveCard(card, fromCollection);
        }
    }
}
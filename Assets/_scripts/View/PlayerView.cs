using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Other.Factory;

namespace View
{
    public class PlayerView : NetworkBehaviour
	{
        #region References

        [SerializeField] PlayerControl owner;
        [SerializeField] Canvas canvas;

        [SerializeField] Button undoButton;
        [SerializeField] Button endTurnButton;

        public GameObject deck;
        public GameObject discard;
        public GameObject hand;
        public GameObject units;
        public GameObject tactic;
        public GameObject play;
        public GameObject limbo;

        [SerializeField] Tactic tacticModel;

        [SerializeField] StatusDisplay level;
        [SerializeField] StatusDisplay handSize;
        [SerializeField] StatusDisplay armour;
        [SerializeField] StatusDisplay movement;
        [SerializeField] StatusDisplay influence;

        public GameObject[] collections;

        #endregion References

        #region Display toggles
        public void Show()
        {
            canvas.enabled = true;
            if (owner.isLocalPlayer) SetButtonsActive();
        }

        public void Hide()
        {
            canvas.enabled = false;
        }
        #endregion Display toggles

        #region Card management
        [ClientRpc]
        public void RpcAddNewCardToDeck(CardId cardId)
        {
            var card = GameController.singleton.cardFactory.CreateCard(cardId);
            card.GetComponent<CardView>().MoveToNewParent(deck.transform, false);
        }

        CardView GetCardFromCollections(CardId card)
        {
            CardView[] list;
            for (int i = 0; i < collections.Length; i++)
            {
                list = collections[i].GetComponentsInChildren<CardView>();
                for (int j = 0; j < list.Length; j++)
                {
                    if (list[j].cardId.identifier == card.identifier)
                    {
                        return list[j];
                    }
                }
            }
            return null;
        }

        [ClientRpc]
        public void RpcMoveCardToLimbo(CardId card)
        {
            var cardView = GetCardFromCollections(card);
            cardView.MoveToNewParent(limbo.transform, true, false);
        }

        [ClientRpc]
        public void RpcMoveCardToHand(CardId card)
        {
            var cardView = GetCardFromCollections(card);
            cardView.MoveToNewParent(hand.transform, owner.isLocalPlayer, owner.isLocalPlayer);
            cardView.cardId.location = GameConstants.Location.Hand;
            
        }

        [ClientRpc]
        public void RpcMoveCardToPlay(CardId card)
        {
            var cardView = GetCardFromCollections(card);
            cardView.MoveToNewParent(play.transform, true);
            cardView.cardId.location = GameConstants.Location.Play;
        }

        [ClientRpc]
        public void RpcMoveCardToDiscard(CardId card)
        {
            var cardView = GetCardFromCollections(card);
            cardView.MoveToNewParent(discard.transform, true);
            cardView.cardId.location = GameConstants.Location.Discard;
        }

        [ClientRpc]
        public void RpcMoveCardToDeck(CardId card)
        {
            var cardView = GetCardFromCollections(card);
            cardView.MoveToNewParent(deck.transform, false);
            cardView.cardId.location = GameConstants.Location.Deck;
        }

        [ClientRpc]
        public void RpcMoveCardToUnits(CardId card)
        {
            var cardView = GetCardFromCollections(card);
            cardView.MoveToNewParent(units.transform, true, true);
            cardView.cardId.location = GameConstants.Location.Units;
        }

        [ClientRpc]
        public void RpcOnTacticChosen(CardId cardId)
        {
            var tacticView = GameController.singleton.cardFactory.CreateCard(cardId).GetComponent<CardView>();
            tacticView.MoveToNewParent(tactic.transform);
            tacticModel.SetTactic(tacticView);
        }
        #endregion

        #region Status Bar Updates
        [ClientRpc]
        public void RpcUpdateMovement(int newValue)
        {
            movement.SetNumber(newValue);
        }

        public void RpcUpdateInfluence(int newValue)
        {
            influence.SetNumber(newValue);
        }
        #endregion

        #region Buttons and commands
        [ClientRpc]
        public void RpcEnableUndo(bool enable)
        {
            undoButton.interactable = enable;
        }

        [ClientRpc]
        public void RpcEnableEndTurn(bool enable)
        {
            endTurnButton.interactable = enable;
        }

        void SetButtonsActive()
        {
            endTurnButton.gameObject.SetActive(true);
            undoButton.gameObject.SetActive(true);
        }

        // Assigned to button.onClick in inspector
        public void UiUndo()
        {
            if (owner.isLocalPlayer) owner.CmdUndo();
        }

        //Assigned to button.onClick in inspector
        public void UiEndTurn()
        {
            if(owner.isLocalPlayer) owner.CmdEndTurn();
        }
        #endregion
    }
}
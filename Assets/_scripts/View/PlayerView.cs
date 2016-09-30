using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Other.Factory;

namespace View
{
    public class PlayerView : NetworkBehaviour
	{
        public NetworkInstanceId ownerId;
        public PlayerControl owner;
        public Canvas canvas;

        public Button undoButton;

        public GameObject deck;
        public GameObject hand;
        public GameObject discard;
        public GameObject units;
        public GameObject tactic;

        public StatusDisplay level;
        public StatusDisplay handSize;
        public StatusDisplay armour;
        public StatusDisplay movement;
        public StatusDisplay influence;

        public GameObject[] collections;

        #region General display toggles
        public void Show()
        {
            canvas.enabled = true;
        }

        public void Hide()
        {
            canvas.enabled = false;
        }

        [ClientRpc]
        public void RpcShow()
        {
            Show();
        }

        [ClientRpc]
        public void RpcHide()
        {
            Hide();
        }
        #endregion

        #region Card management
        [ClientRpc]
        public void RpcAddCardToDeck(CardId cardId)
        {
            GameObject card = GameController.singleton.cardFactory.CreateCard(cardId);
            card.GetComponent<CardView>().MoveToNewParent(deck.transform, false);
        }

        [ClientRpc]
        public void RpcDrawCards(int numberToDraw)
        {
            for (int i = 0; i < numberToDraw; i++)
            {
                if (deck.transform.childCount <= 0)
                    break;

                deck.transform.GetChild(0).GetComponent<CardView>().MoveToNewParent(hand.transform, showFront: owner.isLocalPlayer);
            }
        }

        public CardView GetCardFromCollections(CardId card)
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
        public void RpcMoveCardToHand(CardId card)
        {
            if(owner.isLocalPlayer)
                GetCardFromCollections(card).MoveToNewParent(hand.transform, showFront: true);
            else
                GetCardFromCollections(card).MoveToNewParent(hand.transform, showFront: false);
        }

        [ClientRpc]
        public void RpcMoveCardToDiscard(CardId card)
        {
            GetCardFromCollections(card).MoveToNewParent(discard.transform, showFront: true);
        }

        [ClientRpc]
        public void RpcMoveCardToDeck(CardId card)
        {
            GetCardFromCollections(card).MoveToNewParent(deck.transform, showFront: false);
        }

        [ClientRpc]
        public void RpcMoveCardToUnits(CardId card)
        {
            GetCardFromCollections(card).MoveToNewParent(units.transform, showFront: true);
        }

        [ClientRpc]
        public void RpcOnTacticChosen(CardId cardId)
        {
            GameObject card = GameController.singleton.cardFactory.CreateCard(cardId);
            card.GetComponent<CardView>().MoveToNewParent(tactic.transform);
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

        public void UiUndo()
        {
            owner.CmdUndo();
        }
        #endregion
    }
}
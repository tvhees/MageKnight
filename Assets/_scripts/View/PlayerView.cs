using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace View
{
    public class PlayerView : NetworkBehaviour
    {
        public NetworkInstanceId ownerId;
        public PlayerControl owner;
        public Canvas canvas;

        public Button undoButton;
        public Button endTurnButton;

        public GameObject deck;
        public GameObject hand;
        public GameObject discard;
        public GameObject units;
        public GameObject tactic;
        public GameObject play;

        public Tactic tacticModel;

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

        #endregion General display toggles

        #region Card management

        [ClientRpc]
        public void RpcAddCardToDeck(CardId cardId)
        {
            var card = GameController.singleton.cardFactory.CreateCard(cardId);
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
            if (owner.isLocalPlayer)
                GetCardFromCollections(card).MoveToNewParent(hand.transform, showFront: true);
            else
                GetCardFromCollections(card).MoveToNewParent(hand.transform, showFront: false);
        }

        [ClientRpc]
        public void RpcMoveCardToPlay(CardId card)
        {
            GetCardFromCollections(card).MoveToNewParent(play.transform, showFront: true);
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
            var tacticView = GameController.singleton.cardFactory.CreateCard(cardId).GetComponent<CardView>();
            tacticView.MoveToNewParent(tactic.transform);
            tacticModel.SetTactic(tacticView);
        }

        #endregion Card management

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

        #endregion Status Bar Updates

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

        public void UiUndo()
        {
            owner.CmdUndo();
        }

        public void UiEndTurn()
        {
            owner.CmdEndTurn();
        }

        #endregion Buttons and commands
    }
}
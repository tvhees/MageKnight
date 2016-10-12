﻿using UnityEngine;
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
            Debug.Log(owner.isLocalPlayer);
            if (owner.isLocalPlayer) SetButtonsActive();
        }

        public void Hide()
        {
            canvas.enabled = false;
        }
        #endregion Display toggles

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
        public void RpcMoveCardToHand(CardId card)
        {
            if(owner.isLocalPlayer)
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
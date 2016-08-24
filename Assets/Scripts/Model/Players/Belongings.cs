using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Assertions;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Boardgame.Player
{
    public class Belongings: MonoBehaviour
	{
        public GameObject deckPanel;
        public GameObject handPanel;
        public GameObject discardPanel;

        public void GainCardToDeck(Cards.MovementAndDisplay card)
        {
            GainCardToBelongings(card, deckPanel, showFront: false);
        }

        public void GainCardToDiscard(Cards.MovementAndDisplay card)
        {
            GainCardToBelongings(card, discardPanel);
        }

        public void GainCardToHand(Cards.MovementAndDisplay card)
        {
            GainCardToBelongings(card, handPanel);
        }

        void GainCardToBelongings(Cards.MovementAndDisplay cardController, GameObject target, bool showFront = true)
        {
            cardController.MoveToNewParent(target.transform);
            if (showFront)
                cardController.ShowFront();
            else
                cardController.HideFront();
        }
    }
}
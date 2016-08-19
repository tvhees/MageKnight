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
        public CardHolder deckPanel;
        public CardHolder handPanel;
        public CardHolder discardPanel;

        private CardHolder target;

        public void GainCardToTarget(GameObject card)
        {
            Assert.IsNotNull(target);

            var cardController = card.GetComponent<Cards.MovementAndDisplay>();
            cardController.MoveToNewParent(target.gameObject);

            if (target == deckPanel)
                cardController.HideFront();
            else
                cardController.ShowFront();
        }

        public void SetCardAcquisitionTarget(CardHolder target)
        {
            this.target = target;
            target.ShowDropZone();
        }

        public void TurnOffAcquisitionTarget()
        {
            target = null;
        }
    }
}
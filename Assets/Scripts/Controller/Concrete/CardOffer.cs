using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Cards
{
    public class CardOffer : MonoBehaviour 
	{
        public Cards cards;

        public GameObject actionPanel;
        public GameObject spellPanel;
        public GameObject unitPanel;

        public void FillCardOffers()
        {
            FillActionOffer();
            FillSpellOffer();
            FillUnitOffer();
        }

        public void FillActionOffer()
        {
            int cardsRequired = 3 - actionPanel.transform.childCount;
            for (int i = 0; i < cardsRequired; i++)
            {
                GameObject card = cards.GetAdvancedAction();
                var cardController = card.GetComponent<MovementAndDisplay>();
                cardController.MoveToNewParent(actionPanel);
                cardController.ShowFront();
            }
        }

        public void FillSpellOffer()
        {
            int cardsRequired = 3 - spellPanel.transform.childCount;
            for (int i = 0; i < cardsRequired; i++)
            {
                GameObject card = cards.GetSpell();
                var cardController = card.GetComponent<MovementAndDisplay>();
                cardController.MoveToNewParent(spellPanel);
                cardController.ShowFront();
            }
        }

        public void FillUnitOffer()
        {
            int unitsRequired = Main.Instance.NumberOfPlayers + 2;
            for (int i = 0; i < unitsRequired; i++)
            {
                GameObject card = cards.GetCommonUnit();
                var cardController = card.GetComponent<MovementAndDisplay>();
                cardController.MoveToNewParent(unitPanel);
                cardController.ShowFront();
            }
        }
	}
}
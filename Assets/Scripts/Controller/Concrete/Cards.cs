using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Cards
{
    public class Cards: MonoBehaviour 
	{
        public DeckFactory deckFactory;

        private List<GameObject> wounds;
        private List<GameObject> advancedActions;
        private List<GameObject> spells;
        private List<GameObject> commonUnits;

        public void CreateSharedDecks()
        {
            wounds = deckFactory.CreateDeck(DeckType.Wounds);
            advancedActions = deckFactory.CreateDeck(DeckType.AdvancedActions);
            spells = deckFactory.CreateDeck(DeckType.Spells);
            commonUnits = deckFactory.CreateDeck(DeckType.CommonUnits);
        }

        public List<GameObject> CreatePlayerDeck()
        {
            return deckFactory.CreateDeck(DeckType.Goldyx);
        }

        public GameObject GetCardFromList(List<GameObject> list)
        {
            GameObject card = list.GetLast();
            list.RemoveLast();
            if (card == null)
                Debug.Log("No cards left in " + list.ToString());

            return card;
        }

        public GameObject GetAdvancedAction()
        {
            return GetCardFromList(advancedActions);
        }

        public GameObject GetSpell()
        {
            return GetCardFromList(spells);
        }

        public GameObject GetCommonUnit()
        {
            return GetCardFromList(commonUnits);
        }
    }
}
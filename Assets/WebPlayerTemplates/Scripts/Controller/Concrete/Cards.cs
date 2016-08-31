using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Cards
{
    public class Cards: MonoBehaviour 
	{
        public DeckFactory deckFactory;

        private GameObject cardsHolder;
        private List<GameObject> wounds;
        private List<GameObject> advancedActions;
        private List<GameObject> spells;
        private List<GameObject> commonUnits;

        void Awake()
        {
            Main.cards = this;
            Main.gameSetup.AddListener(CreateSharedDecks);
        }

        public void CreateSharedDecks(Board.Scenario scenario, int numberOfPlayers)
        {
            cardsHolder = new GameObject("Cards Holder");

            wounds = deckFactory.CreateDeck(DeckType.Wounds, cardsHolder);
            advancedActions = deckFactory.CreateDeck(DeckType.AdvancedActions, cardsHolder);
            
            // Include non-cooperative spells here based on scenario
            spells = deckFactory.CreateDeck(DeckType.Spells, cardsHolder);

            commonUnits = deckFactory.CreateDeck(DeckType.CommonUnits, cardsHolder);
        }

        public List<GameObject> CreatePlayerDeck()
        {
            return deckFactory.CreateDeck(DeckType.Goldyx, cardsHolder);
        }

        public GameObject GetCardFromList(List<GameObject> list)
        {
            GameObject card = list.GetLast();

            if (card != null)
            {
                list.RemoveLast();
            }
            else
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
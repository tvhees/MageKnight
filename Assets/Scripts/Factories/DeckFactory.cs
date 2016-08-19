using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Cards
{
    public enum DeckType
    {
        AdvancedActions,
        Spells,
        Artifacts,
        CommonUnits,
        EliteUnits,
        Wounds,
        Goldyx
    }

    public class DeckFactory: MonoBehaviour 
	{
        public DeckDatabase deckDatabase;
        public CardFactory cardFactory;

        public List<GameObject> CreateDeck(DeckType type)
        {
            List<GameObject> listOfCards = new List<GameObject>();
            Deck deckData = deckDatabase.GetScriptableObject(type.ToString());
            GameObject deck = new GameObject(type.ToString());
            deck.transform.SetParent(transform);

            for (int i = 0; i < deckData.cards.Length; i++)
            {
                Card cardData = deckData.cards[i];
                int numberOfCopies = deckData.extraCopies[i] + 1;

                for (int j = 0; j < numberOfCopies; j++)
                {
                    GameObject card = cardFactory.CreateCard(cardData);
                    card.transform.SetParent(deck.transform);
                    listOfCards.Add(card);
                }
            }

            listOfCards.Randomise();

            return listOfCards;
        }
	}
}
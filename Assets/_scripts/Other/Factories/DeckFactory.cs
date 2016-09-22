using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using Other.Data;
using Other.Utility;
using View;

namespace Other.Factory
{
    [RequireComponent(typeof(CardFactory))]
    public class DeckFactory: MonoBehaviour 
	{
        public CardFactory cardFactory;

        public List<GameObject> CreateDeck(Deck deckData)
        {
            List<GameObject> listOfCards = new List<GameObject>();
            for (int i = 0; i < deckData.cards.Length; i++)
            {
                Card cardData = deckData.cards[i];
                int numberOfCopies = deckData.extraCopies[i] + 1;

                for (int j = 0; j < numberOfCopies; j++)
                {
                    GameObject card = cardFactory.CreateCard(new CardId(cardData.name));
                    listOfCards.Add(card);
                }
            }
            
            return listOfCards;
        }
	}
}
﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using Other.Data;
using Other.Utility;
using View;

namespace Other.Factory
{
    public enum DeckType
    {
        AdvancedActions,
        Spells,
        Artifacts,
        CommonUnits,
        EliteUnits,
        Wounds,
        Goldyx,
        Tovak,
        Arythea,
        Norowas
    }

    [RequireComponent(typeof(CardFactory))]
    public class DeckFactory: NetworkBehaviour 
	{
        public CardFactory cardFactory;

        [Server]
        public List<GameObject> ServerCreateDeck(Deck deckData)
        {
            List<GameObject> listOfCards = new List<GameObject>();
            for (int i = 0; i < deckData.cards.Length; i++)
            {
                Card cardData = deckData.cards[i];
                int numberOfCopies = deckData.extraCopies[i] + 1;

                for (int j = 0; j < numberOfCopies; j++)
                {
                    GameObject card = cardFactory.CreateCard(cardData);
                    listOfCards.Add(card);
                }
            }
            
            return listOfCards;
        }
	}
}
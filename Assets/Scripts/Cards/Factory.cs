﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BoardGame
{
    namespace Cards
    {
        public class Factory : Singleton<Factory>
        {

            //************
            // CARD FACTORY
            //************

            // Image and text data sources
            public Sprite[] m_cardImages; // Sprite 0 should be the shared card back
            public TextAsset m_cardXML; // XML file reference

            // List of card dictionaries
            List<Dictionary<string, string>> m_cardList = new List<Dictionary<string, string>>();

            public void LoadXML() // Load text data in to dictionaries
            {
                m_cardList = Loader.LoadCards(m_cardXML);
            }

            private void GiveCardIdentity(Object card, int id) // Set a card to a specific number and matching sprites 
            {
                card.SetID(id);

                card.SetSprites(GetCardFront(id), m_cardImages[0]);
            }

            private Sprite GetCardFront(int id) // Get face sprite matching a card ID
            {
                return m_cardImages[id];
            }

            public Dictionary<string, string> GetCard(int id) // Get text dictionary matching a card id
            {
                return m_cardList[id];
            }

            //************
            // CREATING DECKS
            //************
            public enum DeckType
            {
                SharedDeck,
                PlayerDeck
                // List deck types as necessary
            }

            public GameObject m_cardPrefab;

            public List<Object> CreateDeck(GameObject deck, Camera camera, DeckType type)
            {
                int[] cardNumbers = new DeckList(type).cards;
                cardNumbers.Randomise(false);

                List<Object> deckList = new List<Object>();

                for (int i = 0; i < cardNumbers.Length; i++)
                {
                    Object newCard = deck.transform.InstantiateChild(m_cardPrefab).GetComponent<Object>();
                    GiveCardIdentity(newCard, cardNumbers[i]);
                    deckList.Add(newCard);
                }

                return deckList;
            }
        }

        // This struct contains information on which cards to include in each deck of cards to be created
        [System.Serializable]
        public struct DeckList
        {
            // Cardlists are stored as an array of integers representing card IDs
            public int[] cards;

            // Constructor for making a new decklist
            public DeckList(Factory.DeckType type)
            {
                cards = new int[0];

                // Define the card numbers to include in each type of deck here
                // The same card can be included multiple times
                switch (type)
                {
                    case Factory.DeckType.SharedDeck:
                        cards = new int[16] { 1, 1, 2, 3, 3, 4, 4, 5, 5, 6, 7, 8, 9, 10, 11, 12 };
                        break;
                    case Factory.DeckType.PlayerDeck:
                        cards = new int[16] { 1, 1, 2, 3, 3, 4, 4, 5, 5, 6, 7, 8, 9, 10, 11, 12 };
                        break;
                }
            }
        }
    }
}
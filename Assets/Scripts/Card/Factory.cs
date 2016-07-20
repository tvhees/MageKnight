using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BoardGame
{
    namespace Card
    {
        public class Factory : Singleton<Factory>
        {

            //************
            // CARD FACTORY
            //************

            // Image and text data sources
            public Sprite m_cardBack; // separate sprite for card backs
            public TextAsset m_cardXML; // XML file reference

            // List of card dictionaries
            List<Dictionary<string, string>> m_cardList = new List<Dictionary<string, string>>();

            public void LoadXML() // Load text data in to dictionaries
            {
                m_cardList = Loader.LoadCards(m_cardXML);
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
                Shared,
                Player,
                Wound,
                AdvancedAction,
                Spell,
                Artifact,
                CommonUnit,
                EliteUnit
            }

            public GameObject cardPrefab;

            public void CreatePlayerDeck(Players.Player player, Camera camera, DeckType type)
            {
                // Get the card numbers for all cards in this deck and shuffle the order
                int[] cardNumbers = new DeckList(type).cards;
                cardNumbers.Randomise(false);

                for (int i = 0; i < cardNumbers.Length; i++)
                {
                    Object card = Instantiate(cardPrefab).GetComponent<Object>(); // Create a new card

                    Dictionary<string, string> cardInfo = GetCard(cardNumbers[i]); // Get the card's information from its id number

                    string tempValue;
                    if (cardInfo.TryGetValue("name", out tempValue))
                    {
                        card.cardName = tempValue; // assign card name
                        card.AddSprites(GetCardFront(tempValue), m_cardBack); // assign card sprites
                    }
                    if(cardInfo.TryGetValue("colour", out tempValue)) card.cardColour = tempValue; // assign card colour
                    if(cardInfo.TryGetValue("type", out tempValue)) card.cardType = tempValue; // assign card type

                    card.AddCamera(camera); // Assign the cards this player's camera
                    card.AddEffectButtons(player, cardInfo); // Set up any effects this card has

                    player.MoveCardToDeck(card); // Move the card to the player's deck
                }
            }

            private Sprite GetCardFront(string name)
            {
                Sprite cardFront;
                cardFront = Resources.Load<Sprite>("CardImages/" + name);

                if (cardFront == null)
                    Debug.Log(name);

                return cardFront;
            }

            public List<Object> CreateSharedDeck(GameObject deckHolder, Camera camera, DeckType type, Vector3 deckPosition)
            {
                int[] cardNumbers = new DeckList(type).cards;
                cardNumbers.Randomise(false);

                List<Object> cardList = new List<Object>();

                deckHolder.transform.SetParent(SharedDecks.Instance.transform);
                deckHolder.transform.localPosition = deckPosition;

                for (int i = 0; i < cardNumbers.Length; i++)
                {
                    Object card = deckHolder.transform.InstantiateChild(cardPrefab).GetComponent<Object>(); // Create a new card

                    Dictionary<string, string> cardInfo = GetCard(cardNumbers[i]); // Get the card's information from its id number

                    // Assign the card's name, sprites
                    string cardName;
                    cardInfo.TryGetValue("name", out cardName);
                    card.cardName = cardName;
                    card.AddSprites(GetCardFront(cardName), m_cardBack);

                    card.AddCamera(camera); // Assign the cards the shared camera

                    SharedDecks.Instance.MoveCardToDeck(card, deckHolder, cardList);
                    cardList.Add(card);
                }

                return cardList;
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
                    case Factory.DeckType.Player:
                        cards = new int[16] { 1, 1, 2, 3, 3, 4, 4, 5, 5, 6, 7, 8, 9, 10, 11, 12 };
                        break;
                    case Factory.DeckType.Wound:
                        // Make a big deck of only wounds (card type 0)
                        cards = new int[50];
                        for (int i = 0; i < cards.Length; i++)
                        {
                            cards[i] = 0;
                        }
                        break;
                    case Factory.DeckType.AdvancedAction:
                        cards = new int[28];
                        for (int i = 0; i < cards.Length; i++)
                        {
                            cards[i] = 13 + i;
                        }
                        break;
                    case Factory.DeckType.Spell:
                        break;
                    case Factory.DeckType.Artifact:
                        break;
                    case Factory.DeckType.CommonUnit:
                        break;
                    case Factory.DeckType.EliteUnit:
                        break;
                }
            }
        }
    }
}
using UnityEngine;
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
            public Sprite m_cardBack; // separate sprite for card backs
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

                Dictionary<string, string> cardInfo = GetCard(id);

                string name;
                cardInfo.TryGetValue("name", out name);

                card.SetSprites(GetCardFront(name), m_cardBack);
            }

            private Sprite GetCardFront(string name)
            {
                Sprite cardFront;
                cardFront = Resources.Load<Sprite>("CardImages/" + name);

                if (cardFront == null)
                    Debug.Log(name);

                return cardFront;
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
                int[] cardNumbers = new DeckList(type).cards;
                cardNumbers.Randomise(false);

                for (int i = 0; i < cardNumbers.Length; i++)
                {
                    Object newCard = Instantiate(cardPrefab).GetComponent<Object>();

                    GiveCardIdentity(newCard, cardNumbers[i]);
                    newCard.InitialiseForPlayer(player);
                    player.MoveCardToDeck(newCard);
                }
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
                    Object newCard = deckHolder.transform.InstantiateChild(cardPrefab).GetComponent<Object>();

                    GiveCardIdentity(newCard, cardNumbers[i]);
                    newCard.Initialise(camera);
                    SharedDecks.Instance.MoveCardToDeck(newCard, deckHolder, cardList);
                    cardList.Add(newCard);
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
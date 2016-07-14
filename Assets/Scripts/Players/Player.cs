using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BoardGame
{
    namespace Players
    {
        public class Player : MonoBehaviour
        {
            //**********
            // PREFABS
            //**********

            public GameObject m_boardPrefab;
            public GameObject m_cardHolderPrefab;
            public GameObject m_cameraPrefab;
            public GameObject m_playerCanvasPrefab;

            public int m_playerID { get; private set; }
            public Stats stats { get; private set; }
            public Camera m_playerCamera { get; private set; }

            private GameObject m_playedArea;
            private GameObject m_discard;
            private GameObject m_deck;
            private GameObject m_hand;
            private GameObject m_playerCanvas;

            private Fame m_playerFame;
            private Reputation m_playerReputation;

            private Vector3 m_deckPos = new Vector3(-4f, 0.5f, 0f);
            private Vector3 m_playedAreaPos = new Vector3(-4f, 4f, 0f);

            public void Init(int id)
            {
                m_playerID = id;

                stats = new Stats(5, 2);

                CreatePlayerBoard();

                CreatePlayerDeck();

                RefillHand();
            }

            void CreatePlayerBoard()
            {
                // Instantiate locator/holder objects for this player's cards
                GameObject playerBoard = Instantiate(m_boardPrefab, 10f * Vector3.down, Quaternion.identity) as GameObject; // Create player board to hold all this player's objects
                m_playedArea = playerBoard.transform.InstantiateChild(m_cardHolderPrefab, m_playedAreaPos);
                m_discard = playerBoard.transform.InstantiateChild(m_cardHolderPrefab);
                m_deck = playerBoard.transform.InstantiateChild(m_cardHolderPrefab, m_deckPos);
                m_hand = playerBoard.transform.InstantiateChild(m_cardHolderPrefab);
                m_playerCanvas = playerBoard.transform.InstantiateChild(m_playerCanvasPrefab);

                // Rename for clarity in editor
                playerBoard.name = "Player " + m_playerID + " board";
                m_hand.name = "Hand";
                m_playedArea.name = "Played Cards";
                m_discard.name = "Discard";
                m_deck.name = "Deck";
                m_playerCanvas.name = "Player Canvas";

                // Add camera to show hand
                m_playerCamera = playerBoard.transform.InstantiateChild(m_cameraPrefab, new Vector3(0f, 2f, -10f)).GetComponent<Camera>();
                m_playerCanvas.GetComponent<Canvas>().worldCamera = m_playerCamera;

                // Initialise Fame and Reputation sliders
                m_playerFame = m_playerCanvas.GetComponentInChildren<Fame>();
                m_playerFame.Init();
                m_playerReputation = m_playerCanvas.GetComponentInChildren<Reputation>();
                m_playerReputation.Init();
            }

            // Create a deck only this player can see and start tracking the cards in it
            void CreatePlayerDeck()
            {
                m_cardsInDeck = Cards.Factory.Instance.CreateDeck(m_deck, m_playerCamera, Cards.Factory.DeckType.PlayerDeck);

                for (int i = 0; i < m_cardsInDeck.Count; i++)
                {
                    m_cardsInDeck[i].Init(m_playerID, Location.deck, m_playerCamera);
                }
            }

            //**********
            //CARD TRACKING
            //**********
            private List<Cards.Object> m_cardsInDeck = new List<Cards.Object>();
            public List<Cards.Object> m_cardsInHand = new List<Cards.Object>();
            public List<Cards.Object> m_cardsInPlay = new List<Cards.Object>();
            private List<Cards.Object> m_cardsInDiscard = new List<Cards.Object>();

            public enum Location
            {
                deck,
                hand,
                play,
                discard
            }

            private int m_maxHandSize = 5;
            private float m_cardSlotWidth = 0.75f;

            public void MoveToPlayArea(Cards.Object card)
            {
                ChangeCardLocation(card, Location.play);
                ShiftCardsInHand(card.m_movingObject.m_homePos, m_cardSlotWidth / 2f);
                StartCoroutine(card.m_movingObject.SetHomePos(m_playedArea.transform.position));
            }

            void RefillHand()
            {
                for (int i = m_cardsInHand.Count; i < m_maxHandSize; i++)
                {
                    MoveToHand(m_cardsInDeck[0]);
                }
            }

            public void MoveToHand(Cards.Object card)
            {
                // Shift all cards currently in hand half a space to the left
                ShiftCardsInHand(m_hand.transform.position + 100f * Vector3.left, m_cardSlotWidth / 2f);

                // The new card will go half a card width to the right of the middle of the hand for ever card already in the hand
                Vector3 newCardPos = m_hand.transform.position + Vector3.right * m_cardsInHand.Count * m_cardSlotWidth / 2f;

                MovingObject cardMO = card.m_movingObject;
                StartCoroutine(cardMO.SetHomePos(newCardPos));

                ChangeCardLocation(card, Location.hand);
            }

            void ShiftCardsInHand(Vector3 target, float delta)
            {
                for (int i = 0; i < m_cardsInHand.Count; i++)
                {
                    MovingObject cardMO = m_cardsInHand[i].m_movingObject; // Use the card's moving object script to set it's home position delta units in the target's direction
                    StartCoroutine(cardMO.MoveHomeTowards(target, delta));
                }
            }

            void ChangeCardLocation(Cards.Object card, Location newLocation)
            {
                switch (card.m_location)
                {
                    case Location.deck:
                        m_cardsInDeck.Remove(card);
                        break;
                    case Location.hand:
                        m_cardsInHand.Remove(card);
                        break;
                    case Location.play:
                        m_cardsInPlay.Remove(card);
                        break;
                    case Location.discard:
                        m_cardsInDiscard.Remove(card);
                        break;
                }

                switch (newLocation)
                {
                    case Location.deck:
                        m_cardsInDeck.Add(card);
                        card.transform.SetParent(m_deck.transform);
                        break;
                    case Location.hand:
                        m_cardsInHand.Add(card);
                        card.transform.SetParent(m_hand.transform);
                        break;
                    case Location.play:
                        m_cardsInPlay.Add(card);
                        card.transform.SetParent(m_playedArea.transform);
                        break;
                    case Location.discard:
                        m_cardsInDiscard.Add(card);
                        card.transform.SetParent(m_discard.transform);
                        break;
                }

                card.SetLocation(newLocation);
            }

            //**********
            // COMBAT
            //**********

            public int TakeDamage(Enemy.Attack attack)
            {
                int remaining = attack.strength;
                int woundsTaken = 0;

                if (attack.brutal) // Doubles strength of unblocked attacks
                    remaining *= 2;

                while (remaining > 0)
                {
                    Cards.Object wound = Cards.SharedDecks.Instance.GetWound();
                    MoveToHand(wound);
                    woundsTaken++;
                    remaining -= stats.m_armour;

                    if (attack.poison)
                    {
                        Debug.Log("Add poison wound to discard pile");
                    }
                }

                if (woundsTaken > 1 && attack.paralyze)
                {
                    Debug.Log("Discard hand to Paralyze");
                }

                return woundsTaken;

            }

            //**********
            // FAME AND REPUTATION
            //**********

            public void AddFame(int value)
            {
                m_playerFame.AddFame(value);
            }

            public void AddReputation(int value)
            {
                m_playerReputation.AddReputation(value);
            }
        }

        public struct Stats
        {
            public int m_handSize { get; private set; }
            public int m_armour { get; private set; }

            public Stats(int handSize, int armour)
            {
                m_handSize = handSize;
                m_armour = armour;
            }
        }
    }
}
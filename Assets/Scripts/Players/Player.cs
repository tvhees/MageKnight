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

            private int m_playerID;
            private Camera m_playerCamera;

            private GameObject m_playedArea;
            private GameObject m_discard;
            private GameObject m_deck;
            private GameObject m_hand;

            private Vector3 m_deckPos = new Vector3(-4f, 0.5f, 0f);
            private Vector3 m_playedAreaPos = new Vector3(-4f, 4f, 0f);

            public void Init(int id)
            {
                m_playerID = id;

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

                // Rename for clarity in editor
                playerBoard.name = "Player " + m_playerID + " board";
                m_hand.name = "Hand";
                m_playedArea.name = "Played Cards";
                m_discard.name = "Discard";
                m_deck.name = "Deck";

                // Add camera to show hand
                m_playerCamera = playerBoard.transform.InstantiateChild(m_cameraPrefab, new Vector3(0f, 2f, -10f)).GetComponent<Camera>();
            }

            // Create a deck only this player can see and start tracking the cards in it
            void CreatePlayerDeck()
            {
                m_cardsInDeck = Cards.Factory.Instance.CreateDeck(m_deck, m_playerCamera, Cards.Factory.DeckType.PlayerDeck);

                for (int i = 0; i < m_cardsInDeck.Count; i++)
                {
                    m_cardsInDeck[i].Init(m_playerID, Location.deck, Vector3.zero, m_playerCamera);
                }
            }

            public int GetID()
            {
                return m_playerID;
            }

            //**********
            //CARD TRACKING
            //**********
            private List<Cards.Object> m_cardsInDeck = new List<Cards.Object>();
            private List<Cards.Object> m_cardsInHand = new List<Cards.Object>();
            private List<Cards.Object> m_cardsInPlay = new List<Cards.Object>();
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
                card.SetLocation(Location.play);
                card.GetMover().SetHomePos(m_playedArea.transform.position);
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
                for (int i = 0; i < m_cardsInHand.Count; i++)
                {
                    ShiftCardHome(m_cardsInHand[i], m_hand.transform.position + 100f * Vector3.left, m_cardSlotWidth / 2f);
                }

                card.transform.SetParent(m_hand.transform);
                card.GetMover().SetHomePos(m_hand.transform.position + Vector3.right * m_cardsInHand.Count * m_cardSlotWidth / 2f);
                ChangeCardLocation(card, Location.hand);
                m_cardsInHand.Add(card);
            }

            void ShiftCardHome(Cards.Object card, Vector3 target, float delta)
            {
                card.GetMover().SetHomePos(Vector3.MoveTowards(card.GetMover().GetHomePos(), target, delta));
            }

            void ChangeCardLocation(Cards.Object card, Location newLocation)
            {
                switch (card.GetLocation())
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

                card.SetLocation(newLocation);
            }
        }
    }
}
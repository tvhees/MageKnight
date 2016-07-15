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

            public GameObject boardPrefab;
            public GameObject cardHolderPrefab;
            public GameObject cameraPrefab;
            public GameObject playerCanvasPrefab;

            public int playerID { get; private set; }
            public Stats stats { get; private set; }
            public Camera playerCamera { get; private set; }

            private GameObject playedArea;
            private GameObject discard;
            private GameObject deck;
            private GameObject hand;
            private GameObject playerCanvas;

            private Fame m_playerFame;
            private Reputation m_playerReputation;

            private Vector3 deckPos = new Vector3(-4f, 0.1f, 0f);
            private Vector3 discardPos = new Vector3(-4, 2f, 0f);
            private Vector3 playedAreaPos = new Vector3(-4f, 4f, 0f);

            void Update()
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    Enemy.Attack newAttack = new Enemy.Attack();
                    newAttack.strength = 4;
                    TakeDamage(newAttack);
                }
            }

            public void Init(int id)
            {
                playerID = id;

                stats = new Stats(5, 2);

                GetComponent<MovingObject>().SetSpeed(25);

                CreatePlayerBoard();

                CreatePlayerDeck();

                RefillHand();
            }

            void CreatePlayerBoard()
            {
                // Instantiate locator/holder objects for this player's cards
                GameObject playerBoard = Instantiate(boardPrefab, 10f * Vector3.down, Quaternion.identity) as GameObject; // Create player board to hold all this player's objects
                playedArea = playerBoard.transform.InstantiateChild(cardHolderPrefab, playedAreaPos);
                discard = playerBoard.transform.InstantiateChild(cardHolderPrefab, discardPos);
                deck = playerBoard.transform.InstantiateChild(cardHolderPrefab, deckPos);
                hand = playerBoard.transform.InstantiateChild(cardHolderPrefab);
                playerCanvas = playerBoard.transform.InstantiateChild(playerCanvasPrefab);

                // Rename for clarity in editor
                playerBoard.name = "Player " + playerID + " board";
                hand.name = "Hand";
                playedArea.name = "Played Cards";
                discard.name = "Discard";
                deck.name = "Deck";
                playerCanvas.name = "Player Canvas";

                // Add camera to show hand
                playerCamera = playerBoard.transform.InstantiateChild(cameraPrefab, new Vector3(0f, 2f, -10f)).GetComponent<Camera>();
                playerCanvas.GetComponent<Canvas>().worldCamera = playerCamera;

                // Initialise Fame and Reputation sliders
                m_playerFame = playerCanvas.GetComponentInChildren<Fame>();
                m_playerFame.Init();
                m_playerReputation = playerCanvas.GetComponentInChildren<Reputation>();
                m_playerReputation.Init();
            }

            // Create a deck only this player can see and start tracking the cards in it
            void CreatePlayerDeck()
            {
                m_cardsInDeck = Cards.Factory.Instance.CreateDeck(deck, playerCamera, Cards.Factory.DeckType.PlayerDeck);

                for (int i = 0; i < m_cardsInDeck.Count; i++)
                {
                    m_cardsInDeck[i].InitialiseForPlayer(playerID, Location.deck, playerCamera);
                }
            }

            //**********
            //CARD TRACKING
            //**********
            private List<Cards.Object> m_cardsInDeck = new List<Cards.Object>();
            public List<Cards.Object> m_cardsInHand = new List<Cards.Object>();
            public List<Cards.Object> cardsInPlay = new List<Cards.Object>();
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
                ShiftCardsInHand(card.movingObject.homePos, m_cardSlotWidth / 2f);
                StartCoroutine(card.movingObject.SetHomePos(playedArea.transform.position));
            }

            public void MoveToDiscard(Cards.Object card)
            {
                if(card.location == Location.hand)
                    ShiftCardsInHand(card.movingObject.homePos, m_cardSlotWidth / 2f);
                ChangeCardLocation(card, Location.discard);
                StartCoroutine(card.movingObject.SetHomePos(discard.transform.position));
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
                ShiftCardsInHand(hand.transform.position + 100f * Vector3.left, m_cardSlotWidth / 2f);

                // The new card will go half a card width to the right of the middle of the hand for ever card already in the hand
                Vector3 newCardPos = hand.transform.position + Vector3.right * m_cardsInHand.Count * m_cardSlotWidth / 2f;

                ChangeCardLocation(card, Location.hand);

                MovingObject cardMO = card.movingObject;
                StartCoroutine(cardMO.SetHomePos(newCardPos));
            }

            void ShiftCardsInHand(Vector3 target, float delta)
            {
                for (int i = 0; i < m_cardsInHand.Count; i++)
                {
                    MovingObject cardMO = m_cardsInHand[i].movingObject; // Use the card's moving object script to set it's home position delta units in the target's direction
                    StartCoroutine(cardMO.MoveHomeTowards(target, delta));
                }
            }

            void ChangeCardLocation(Cards.Object card, Location newLocation)
            {
                switch (card.location)
                {
                    case Location.deck:
                        m_cardsInDeck.Remove(card);
                        break;
                    case Location.hand:
                        m_cardsInHand.Remove(card);
                        break;
                    case Location.play:
                        cardsInPlay.Remove(card);
                        break;
                    case Location.discard:
                        m_cardsInDiscard.Remove(card);
                        break;
                }

                switch (newLocation)
                {
                    case Location.deck:
                        m_cardsInDeck.Add(card);
                        card.transform.SetParent(deck.transform);
                        break;
                    case Location.hand:
                        m_cardsInHand.Add(card);
                        card.transform.SetParent(hand.transform);
                        break;
                    case Location.play:
                        cardsInPlay.Add(card);
                        card.transform.SetParent(playedArea.transform);
                        break;
                    case Location.discard:
                        m_cardsInDiscard.Add(card);
                        card.transform.SetParent(discard.transform);
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
                    wound.InitialiseForPlayer(playerID, Location.hand, playerCamera, false);
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

            //**********
            // END OF TURN
            //**********

            public void EndOfTurn()
            {
                CleanUpPlayedArea();
                RefillHand();
            }

            void CleanUpPlayedArea()
            {
                int cardsToDiscard = cardsInPlay.Count;
                for(int i = 0; i < cardsToDiscard; i++)
                    MoveToDiscard(cardsInPlay[0]);
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
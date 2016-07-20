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

            public int id { get; private set; }
            public Stats stats { get; private set; }
            public Camera playerCamera { get; private set; }

            private GameObject playedArea;
            private GameObject discard;
            private GameObject deck;
            private GameObject hand;
            private GameObject playerCanvas;

            private Fame fame;
            private Reputation reputation;

            private Vector3 deckPosition = new Vector3(-5.65f, 0.1f, 3f);
            private Vector3 discardPosition = new Vector3(-5.65f, 2f, 3f);
            private Vector3 playedAreaPosition = new Vector3(-5.65f, 4f, 3f);

            public HexGrid.Manager currentTile;

            public void Init(int id)
            {
                this.id = id;

                stats = new Stats(5, 2);

                GetComponent<MovingObject>().SetSpeed(25);

                CreatePlayerBoard();

                CreatePlayerDeck();

                RefillHand();

                SetCurrentTile();
            }

            void SetCurrentTile()
            {
                currentTile = AdjacencyChecker.OverlapSphereForType<HexGrid.Manager>(transform.position, 0.3f, "Board")[0];
            }

            void CreatePlayerBoard()
            {
                // Instantiate locator/holder objects for this player's cards
                GameObject playerBoard = Instantiate(boardPrefab, 10f * Vector3.down, Quaternion.identity) as GameObject; // Create player board to hold all this player's objects
                playedArea = playerBoard.transform.InstantiateChild(cardHolderPrefab, playedAreaPosition);
                discard = playerBoard.transform.InstantiateChild(cardHolderPrefab, discardPosition);
                deck = playerBoard.transform.InstantiateChild(cardHolderPrefab, deckPosition);
                hand = playerBoard.transform.InstantiateChild(cardHolderPrefab);
                playerCanvas = playerBoard.transform.InstantiateChild(playerCanvasPrefab);

                // Rename for clarity in editor
                playerBoard.name = "Player " + id + " board";
                hand.name = "Hand";
                playedArea.name = "Played Cards";
                discard.name = "Discard";
                deck.name = "Deck";
                playerCanvas.name = "Player Canvas";

                // Add camera to show hand
                playerCamera = playerBoard.transform.InstantiateChild(cameraPrefab, new Vector3(0f, 2f, -10f)).GetComponent<Camera>();
                playerCanvas.GetComponent<Canvas>().worldCamera = playerCamera;

                // Initialise Fame and Reputation sliders
                fame = playerCanvas.GetComponentInChildren<Fame>();
                fame.Init();
                reputation = playerCanvas.GetComponentInChildren<Reputation>();
                reputation.Init();
            }

            // Create a deck only this player can see and start tracking the cards in it
            void CreatePlayerDeck()
            {
                Card.Factory.Instance.CreatePlayerDeck(this, playerCamera, Card.Factory.DeckType.Player);
            }

            //**********
            //CARD TRACKING
            //**********
            private List<Card.Object> cardsInDeck = new List<Card.Object>();
            public List<Card.Object> cardsInHand = new List<Card.Object>();
            public List<Card.Object> cardsInPlay = new List<Card.Object>();
            private List<Card.Object> cardsInDiscard = new List<Card.Object>();

            private int maxHandSize = 5;
            private float m_cardSlotWidth = 0.75f;

            public void MoveCardToPlayArea(Card.Object card)
            {
                Vector3 nextPositionInPlayArea = playedArea.transform.position + cardsInPlay.Count * new Vector3(0.5f, 0f, 0f);
                ChangeCardLocation(card, Card.Object.Location.play);
                ShiftCardsInHand(card.movingObject.homePos, m_cardSlotWidth / 2f);
                StartCoroutine(card.movingObject.SetHomePos(nextPositionInPlayArea));
            }

            public void MoveCardToDiscard(Card.Object card)
            {
                if(card.location == Card.Object.Location.hand)
                    ShiftCardsInHand(card.movingObject.homePos, m_cardSlotWidth / 2f);

                Vector3 topOfDiscardPile = discard.transform.position + cardsInDiscard.Count * new Vector3(0f, 0f, -0.05f);

                ChangeCardLocation(card, Card.Object.Location.discard);
                StartCoroutine(card.movingObject.SetHomePos(topOfDiscardPile));
            }

            public void MoveCardToDeck(Card.Object card)
            {
                if (card.location == Card.Object.Location.hand)
                    ShiftCardsInHand(card.movingObject.homePos, m_cardSlotWidth / 2f);

                Vector3 topOfDeck = deck.transform.position + cardsInDeck.Count * new Vector3(0f, 0f, -0.05f);

                ChangeCardLocation(card, Card.Object.Location.deck);
                StartCoroutine(card.movingObject.SetHomePos(topOfDeck));
            }

            public void ThrowAwayCard(Card.Object card)
            {
                if (card.location == Card.Object.Location.hand)
                    ShiftCardsInHand(card.movingObject.homePos, m_cardSlotWidth / 2f);

                ChangeCardLocation(card, Card.Object.Location.throwAway);
            }

            public void DrawCards(int numberToDraw = 1)
            {
                if (numberToDraw <= 0) return;

                for (int i = 0; i < numberToDraw; i++)
                { if (cardsInDeck.Count > 0)
                        MoveToHand(cardsInDeck[0]);
                }
            }

            void RefillHand()
            {
                DrawCards(maxHandSize - cardsInHand.Count);
            }

            public void MoveToHand(Card.Object card)
            {
                // Shift all cards currently in hand half a space to the left
                ShiftCardsInHand(hand.transform.position + 100f * Vector3.left, m_cardSlotWidth / 2f);

                // The new card will go half a card width to the right of the middle of the hand for ever card already in the hand
                Vector3 newCardPos = hand.transform.position + Vector3.right * cardsInHand.Count * m_cardSlotWidth / 2f;

                ChangeCardLocation(card, Card.Object.Location.hand);

                MovingObject cardMO = card.movingObject;
                StartCoroutine(cardMO.SetHomePos(newCardPos));
            }

            void ShiftCardsInHand(Vector3 target, float delta)
            {
                for (int i = 0; i < cardsInHand.Count; i++)
                {
                    MovingObject cardMO = cardsInHand[i].movingObject; // Use the card's moving object script to set it's home position delta units in the target's direction
                    StartCoroutine(cardMO.MoveHomeTowards(target, delta));
                }
            }

            void ChangeCardLocation(Card.Object card, Card.Object.Location newLocation)
            {
                switch (card.location)
                {
                    case Card.Object.Location.deck:
                        cardsInDeck.Remove(card);
                        break;
                    case Card.Object.Location.hand:
                        cardsInHand.Remove(card);
                        break;
                    case Card.Object.Location.play:
                        cardsInPlay.Remove(card);
                        break;
                    case Card.Object.Location.discard:
                        cardsInDiscard.Remove(card);
                        break;
                }

                switch (newLocation)
                {
                    case Card.Object.Location.deck:
                        cardsInDeck.Add(card);
                        card.transform.SetParent(deck.transform);
                        card.DisableEffectButtons();
                        break;
                    case Card.Object.Location.hand:
                        cardsInHand.Add(card);
                        card.transform.SetParent(hand.transform);
                        card.EnableEffectButtons();
                        break;
                    case Card.Object.Location.play:
                        cardsInPlay.Add(card);
                        card.transform.SetParent(playedArea.transform);
                        card.DisableEffectButtons();
                        break;
                    case Card.Object.Location.discard:
                        cardsInDiscard.Add(card);
                        card.transform.SetParent(discard.transform);
                        card.DisableEffectButtons();
                        break;
                    case Card.Object.Location.throwAway:
                        card.DisableEffectButtons();
                        break;
                }

                card.SetLocation(newLocation);
            }

            //**********
            // COMBAT
            //**********

            public int WoundsDueToAttack(Enemy.Attack attack)
            {
                int remainingDamage = attack.strength;
                int woundsTaken = 0;

                if (attack.brutal) // Doubles strength of unblocked attacks
                    remainingDamage *= 2;

                while (remainingDamage > 0)
                {
                    TakeWound(Card.Object.Location.hand);

                    woundsTaken++;

                    remainingDamage -= stats.armour; // Subtract our current armour from the remaining damage total

                    if (attack.poison)
                    {
                        TakeWound(Card.Object.Location.discard);
                    }
                }

                if (woundsTaken > 1 && attack.paralyze)
                {
                    Paralyze();
                }

                return woundsTaken;
            }

            void TakeWound(Card.Object.Location placeToSendWound)
            {
                Card.Object wound = Card.SharedDecks.Instance.GetWound();
                //wound.AddEffectButtons(this);
                wound.AddCamera(playerCamera);

                if (placeToSendWound == Card.Object.Location.hand)
                    MoveToHand(wound);
                else if (placeToSendWound == Card.Object.Location.discard)
                    MoveCardToDiscard(wound);
            }

            void Paralyze()
            {
                for (int i = cardsInHand.Count; i > 0; i--)
                {
                    Card.Object card = cardsInHand[i - 1];

                    if (card.cardName != "Wound") MoveCardToDiscard(card);
                }
            }

            //**********
            // FAME AND REPUTATION
            //**********

            public void AddFame(int value)
            {
                fame.AddFame(value);
            }

            public void AddReputation(int value)
            {
                reputation.AddReputation(value);
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
                    MoveCardToDiscard(cardsInPlay[0]);
            }
        }

        public struct Stats
        {
            public int handSize { get; private set; }
            public int armour { get; private set; }

            public Stats(int handSize, int armour)
            {
                this.handSize = handSize;
                this.armour = armour;
            }
        }
    }
}
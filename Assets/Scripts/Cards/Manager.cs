using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BoardGame
{
    namespace Cards
    {
        public class Manager : Singleton<Manager>
        {

            //************
            // INITIALISATION
            //************

            public void Init()
            {
                Factory.Instance.LoadXML();
                CreateDecks();
            }

            void CreateDecks()
            {
                // Create a shared deck all players can see
                // cardFactory.CreateDeck(new GameObject("SharedDeck"), Game.Manager.Instance.m_sharedCamera, Factory.DeckType.SharedDeck);
            }

            //************
            // CARD EFFECTS
            //************

            private int m_effectPlayerID; // player the effect belongs to
            private Object m_effectCard; // card the effect belongs to

            private int i = 0;

            // List of effect types a card can have
            public enum EffectType
            {
                tap,
                _1,
                _2
                // List effect types as necessary
            }

            public void ProcessEffect(int playerID, Object card, int cardID, EffectType effectType)
            {
                // Store reference to the card and player
                m_effectCard = card;
                m_effectPlayerID = playerID;

                // Get the dictionary entry for this card
                Dictionary<string, string> cardInfo = Factory.Instance.GetCard(cardID);
                string name;
                string value;

                if (effectType == EffectType.tap)
                {
                    name = "BasicChoice";
                    value = "1";
                }
                else
                {
                    // Find the type and value of the effect in the dictionary - search term depends on if we
                    // are using the strong or weak effect of the card
                    cardInfo.TryGetValue("effect" + effectType.ToString(), out name);
                    cardInfo.TryGetValue("value" + effectType.ToString(), out value);
                }
                // Start the named method
                EffectMethods.Instance.RunCoroutine(name, value);
            }

            public void UsedEffect(bool tap = false)
            {
                if (tap)
                {
                    StartCoroutine(m_effectCard.GetMover().SetTargetRot(Quaternion.Euler(0f, 0f, 90f)));
                }

                // By default, successfully using a card sends it to the play area
                Game.Manager.Instance.GetCurrentPlayer().MoveToPlayArea(m_effectCard);
            }

            public void FailedEffect()
            {
                i++;
                Debug.Log("failed " + i);
                // Place methods for cards that can't be used here
            }

            //************
            // CREATING DECKS
            //************
            public enum DeckType
            {
                deck1,
                deck2
                // List deck types as necessary
            }
        }
    }
}
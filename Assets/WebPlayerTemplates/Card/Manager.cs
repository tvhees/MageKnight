using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
    namespace Card
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
                SharedDecks.Instance.Init();
            }

            //************
            // CARD EFFECTS
            //************

            private int m_effectPlayerID; // player the effect belongs to
            private Object m_effectCard; // card the effect belongs to

            private int i = 0;

            public void UsedEffect(bool tap = false)
            {
                if (tap)
                {
                    StartCoroutine(m_effectCard.movingObject.SetTargetRot(Quaternion.Euler(0f, 0f, 90f)));
                }

                // By default, successfully using a card sends it to the play area
                Game.GetCurrentPlayer().MoveCardToPlayArea(m_effectCard);
            }

            public void FailedEffect()
            {
                i++;
                Debug.Log("failed " + i);
                // Place methods for cards that can't be used here
            }
        }
    }
}
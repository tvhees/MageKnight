using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BoardGame
{
	namespace Cards
    {
		public class SharedDecks : Singleton<SharedDecks> 
		{
            // Shared camera for showing decks
            public Camera m_sharedCamera;

            // Prefab for scene objects
            public GameObject m_cardHolderPrefab;

            // Scene objects to keep cards organised
            private GameObject m_woundDeck;

            private List<Object> m_woundCards;

            public void Init()
            {
                m_woundDeck = transform.InstantiateChild(m_cardHolderPrefab);
                m_woundDeck.name = "Wound Deck";
                m_woundCards = CreateSharedDeck(m_woundDeck, Factory.DeckType.WoundDeck);
            }

            List<Object> CreateSharedDeck(GameObject deckHolder, Factory.DeckType deckType)
            {
                List<Object> deckList = Factory.Instance.CreateDeck(deckHolder, m_sharedCamera, deckType);

                for (int i = 0; i < deckList.Count; i++)
                {
                    deckList[i].Initialise(m_sharedCamera);
                }

                return deckList;
            }

            public Object GetWound()
            {
                Object wound = m_woundCards.GetLast();
                m_woundCards.RemoveLast();

                return wound;
            }
        }
	}
}
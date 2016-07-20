using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BoardGame
{
	namespace Card
    {
		public class SharedDecks : Singleton<SharedDecks> 
		{
            // Shared camera for showing decks
            public Camera sharedCamera;

            // Deck object reference lists
            private GameObject woundHolder;

            // Card reference lists
            private List<Object> woundDeck;
            private List<Object> advancedActionDeck;
            private List<Object> spellDeck;
            private List<Object> artifactDeck;
            private List<Object> commonUnitDeck;
            private List<Object> eliteUnitDeck;

            // Vectors for locating each deck
            private Vector3 woundPosition = new Vector3(-10f, 2f, 3f);
            private Vector3 advancedActionPosition = new Vector3(-2f, 4f, 3f);

            // Vector offset to create a 3D card 'stack'
            private Vector3 stackingOffset = new Vector3(0f, 0f, -0.05f);

            public void Init()
            {
                woundDeck = Factory.Instance.CreateSharedDeck(woundHolder = new GameObject("Wound Deck"), sharedCamera, Factory.DeckType.Wound, woundPosition);
                //advancedActionDeck = Factory.Instance.CreateSharedDeck(new GameObject("Advanced Action Deck"), sharedCamera, Factory.DeckType.AdvancedAction, advancedActionPosition);
                //spellDeck = Factory.Instance.CreateSharedDeck(new GameObject("Spell Deck"), sharedCamera, Factory.DeckType.Wound);
                //artifactDeck = Factory.Instance.CreateSharedDeck(new GameObject("Artifact Deck"), sharedCamera, Factory.DeckType.Wound);
                //commonUnitDeck = Factory.Instance.CreateSharedDeck(new GameObject("Common Unit Deck"), sharedCamera, Factory.DeckType.Wound);
                //eliteUnitDeck = Factory.Instance.CreateSharedDeck(new GameObject("Elite Unit Deck"), sharedCamera, Factory.DeckType.Wound);
            }

            public void MoveCardToDeck(Object card, GameObject deck, List<Object> cardsInDeck)
            {
                Vector3 topOfDeck = deck.transform.position + cardsInDeck.Count * stackingOffset;
                ChangeCardLocation(card, Object.Location.deck);
                StartCoroutine(card.movingObject.SetHomePos(topOfDeck));
            }

            void ChangeCardLocation(Object card, Object.Location newLocation)
            {
                card.SetLocation(newLocation);
            }

            public Object GetWound()
            {
                Object wound = woundDeck.GetLast();
                woundDeck.RemoveLast();

                return wound;
            }

            public void ReturnWound(Object wound)
            {
                MoveCardToDeck(wound, woundHolder, woundDeck);
            }
        }
	}
}
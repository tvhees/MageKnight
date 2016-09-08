using UnityEngine;
using UnityEngine.Networking;
using Other.Data;
using View;

namespace Other.Factory
{
    [RequireComponent(typeof(DeckFactory))]
    public class CardFactory : NetworkBehaviour
    {
        public Sprite cardBack;
        public GameObject cardPrefab;

        public GameObject CreateCard(Card cardData)
        {
            GameObject card = Instantiate(cardPrefab);
            NetworkServer.Spawn(card);

            var cardView = card.GetComponent<CardView>();
            cardView.cardFront = GetCardFront(cardData.name);
            cardView.cardBack = cardBack;

            card.name = cardData.name;
            cardView.ShowFront();

            return card;
        }

        private Sprite GetCardFront(string name)
        {
            name = name.Replace(" ", "");
            name = name.ToLower();

            Sprite cardFront = Resources.Load<Sprite>("CardImages/" + name);

            if (cardFront == null)
                Debug.Log(name);

            return cardFront;
        }
    }
}
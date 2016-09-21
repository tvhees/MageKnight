using UnityEngine;
using UnityEngine.Networking;
using Other.Data;
using View;

namespace Other.Factory
{
    public class CardFactory : MonoBehaviour
    {
        public Sprite cardBack;
        public GameObject cardPrefab;

        public GameObject CreateCard(CardId cardId)
        {
            GameObject card = Instantiate(cardPrefab);
            card.name = cardId.name;

            var cardView = card.GetComponent<CardView>();
            cardView.SetCardImages(card.name);
            cardView.Show();

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
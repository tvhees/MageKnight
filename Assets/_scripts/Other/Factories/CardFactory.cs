using UnityEngine;
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
            var cardView = card.GetComponent<CardView>();
            cardView.cardId = cardId;
            cardView.SetCardImages();
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
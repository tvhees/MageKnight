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
            var card = Instantiate(cardPrefab);
            var cardView = card.GetComponent<CardView>();
            cardView.cardId = cardId;
            cardView.SetCardImages();

            return card;
        }
    }
}
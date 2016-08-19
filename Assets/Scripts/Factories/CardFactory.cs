using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Cards
{
    public class CardFactory : MonoBehaviour
    {
        public Sprite cardBack;
        public GameObject cardPrefab;

        public GameObject CreateCard(Card cardData)
        {
            GameObject card = Instantiate(cardPrefab);
            card.GetComponent<MovementAndDisplay>().cardFront = GetCardFront(cardData.name);
            card.name = cardData.name;

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
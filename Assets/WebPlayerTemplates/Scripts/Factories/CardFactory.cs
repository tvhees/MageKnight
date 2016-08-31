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
            var moveAndDisp = card.GetComponent<MovementAndDisplay>();
            moveAndDisp.cardFront = GetCardFront(cardData.name);
            moveAndDisp.type = cardData.type;
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

        private void SetStandardCost(Card cardData, Acquirable acquirable)
        {
            switch (cardData.type)
            {
                case Card.Type.Action:
                    break;
                case Card.Type.Spell:
                    break;
                case Card.Type.CommonUnit:
                case Card.Type.EliteUnit:
                    break;
            }
        }
    }
}
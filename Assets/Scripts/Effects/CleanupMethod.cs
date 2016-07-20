using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BoardGame
{
	namespace Effect
    {
		public class CleanupMethod : MonoBehaviour 
		{
            public void Success()
            {
                Card.Object card = GetComponentInParent<Card.Object>();
                if (card.cardType == "basic" || card.cardType == "advanced" || card.cardType == "spell") PlayAsNormal(card);

                if (card.cardType == "artifact")
                {
                    Type type = GetComponent<Type>();
                    if (type.effectType == "weak") PlayAsNormal(card);
                    else if (type.effectType == "strong") ThrowAway(card);
                }

                if (card.cardType == "common" || card.cardType == "elite") ExhaustUnit(card);
            }

            public void PlayAsNormal(Card.Object card)
            {
                GetComponent<Button>().player.MoveCardToPlayArea(card);
            }

            public void Discard(Card.Object card)
            {
                GetComponent<Button>().player.MoveCardToDiscard(card);
            }

            public void ThrowAway(Card.Object card)
            {
                if (card.cardType == "wound")
                    Card.SharedDecks.Instance.ReturnWound(card);
            }

            void ExhaustUnit(Card.Object card)
            {

            }

		}
	}
}
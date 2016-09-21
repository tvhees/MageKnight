using UnityEngine;
using UnityEngine.Networking;
using Other.Factory;

namespace View
{
    public class PlayerView : NetworkBehaviour
	{
        public NetworkInstanceId ownerId;
        public GameObject deck;
        public GameObject hand;
        public GameObject discard;
        public GameObject units;
        public GameObject tactic;

        [ClientRpc]
        public void RpcAddCardToDeck(CardId cardId)
        {
            GameObject card = GameController.singleton.cardFactory.CreateCard(cardId);
            card.GetComponent<CardView>().MoveToNewParent(deck.transform, false);
        }

        [ClientRpc]
        public void RpcDrawCards(int numberToDraw)
        {
            for (int i = 0; i < numberToDraw; i++)
            {
                if (deck.transform.childCount <= 0)
                    break;

                deck.transform.GetChild(0).GetComponent<CardView>().MoveToNewParent(hand.transform);
            }
        }

        [ClientRpc]
        public void RpcOnTacticChosen(CardId cardId)
        {
            GameObject card = GameController.singleton.cardFactory.CreateCard(cardId);
            card.GetComponent<CardView>().MoveToNewParent(tactic.transform);
        }
    }
}
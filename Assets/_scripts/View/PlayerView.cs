using UnityEngine;
using UnityEngine.Networking;
using Other.Factory;

namespace View
{
    public class PlayerView : NetworkBehaviour
	{
        public NetworkInstanceId ownerId;
        public PlayerControl owner;

        public GameObject deck;
        public GameObject hand;
        public GameObject discard;
        public GameObject units;
        public GameObject tactic;
        public Canvas canvas;

        public void Show()
        {
            canvas.enabled = true;
        }

        public void Hide()
        {
            canvas.enabled = false;
        }

        [ClientRpc]
        public void RpcShow()
        {
            Show();
        }

        [ClientRpc]
        public void RpcHide()
        {
            Hide();
        }

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

                deck.transform.GetChild(0).GetComponent<CardView>().MoveToNewParent(hand.transform, showFront: owner.isLocalPlayer);
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
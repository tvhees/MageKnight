using UnityEngine;
using UnityEngine.Networking;

namespace Model
{
    public class PlayerView : NetworkBehaviour
	{
        public GameObject holderPrefab;

        public GameObject deck;
        public GameObject hand;
        public GameObject discard;
        public GameObject units;

        [Server]
        public void ServerSpawnCardHolders()
        {
            deck = Instantiate(holderPrefab);
            NetworkServer.Spawn(deck);
            transform.ServerSetChild(deck.transform);
        }
    }
}
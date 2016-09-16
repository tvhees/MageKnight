using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using View;
using Other.Data;
using Other.Factory;

public class CardSetup : NetworkBehaviour {

    public StateController stateController;
    public DeckFactory deckFactory;
    public GameObject holderPrefab;

    [ServerCallback]
    void OnEnable()
    {
        ServerCreateDecks();

        stateController.ServerChangeState(stateController.tacticSelect);
    }

    [Server]
    public void ServerCreateDecks()
    {
        PlayerControl[] players = GameController.singleton.players.ToArray();
        for (int i = 0; i < players.Length; i++)
        {
            ServerCreatePlayerDeck(players[i]);
        }
    }

    #region Private
    [Server]
    void ServerCreatePlayerDeck(PlayerControl player)
    {
        List<GameObject> deckList = deckFactory.ServerCreateDeck(player.character.deck);
        deckList.Randomise();
        for (int i = 0; i < deckList.Count; i++)
        {
            player.ServerMoveCardToDeck(deckList[i]);
        }
    }
    [Server]
    void ServerCreateCommonDeck(Deck deckData)
    {
        GameObject deckHolder = Instantiate(holderPrefab);
        deckHolder.name = deckData.name;
        stateController.ServerSpawnObject(deckHolder);

        List<GameObject> deckList = deckFactory.ServerCreateDeck(deckData);
        for (int i = 0; i < deckList.Count; i++)
        {
            deckList[i].GetComponent<CardView>().MoveToNewParent(deckHolder.transform);
        }
    }
    #endregion
}

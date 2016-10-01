using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class StateController : NetworkBehaviour {

    public GameObject characterSelect;
    public GameObject boardSetup;
    public GameObject tacticSelect;
    public GameObject roundSetup;
    public GameObject turnSetup;
    public GameObject startOfTurn;
    public GameObject movement;

    public GameObject[] gameStates;

    [SyncVar(hook = "OnStateIndexChanged")]
    public int stateIndex;
    public GameObject gameState;
    public GameObject lastState;

    [Server]
    public void ServerChangeToState(GameConstants.GameState newState)
    {
        stateIndex = (int)newState;
        ServerChangeToState(gameStates[(int)newState]);
    }

    [Server]
    public void ServerChangeToState(GameObject newState)
    {
        stateIndex = newState.transform.GetSiblingIndex();
        ChangeToState(newState);
    }

    void ChangeToState(GameObject newState)
    {
        if (gameState == newState)
            return;

        if (gameState != null)
            gameState.SetActive(false);

        gameState = newState;
        gameState.SetActive(true);
    }

    [Client]
    public void OnStateIndexChanged(int newIndex)
    {
        stateIndex = newIndex;
        EventManager.stateChanged.Invoke(gameStates[newIndex]);
    }

    [Server]
    public void ServerSpawnObject(GameObject obj)
    {
        NetworkServer.Spawn(obj);
    }
}

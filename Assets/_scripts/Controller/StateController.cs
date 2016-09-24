using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class StateController : NetworkBehaviour {

    public GameObject characterSelect;
    public GameObject boardSetup;
    public GameObject tacticSelect;
    public GameObject startOfRound;
    public GameObject startOfTurn;
    public GameObject movement;

    [SyncVar(hook = "OnStateIndexChanged")]
    public int stateIndex;
    public GameObject gameState;
    public GameObject lastState;

    [Server]
    public void ServerChangeState(GameObject newState)
    {
        ChangeState(newState);
        stateIndex = gameState.transform.GetSiblingIndex();
    }

    void ChangeState(GameObject newState)
    {
        if (gameState == newState)
            return;

        if (gameState != null)
            gameState.SetActive(false);

        gameState = newState;
        gameState.SetActive(true);

        EventManager.stateChanged.Invoke(gameState);
    }

    [Client]
    public void OnStateIndexChanged(int newIndex)
    {
        stateIndex = newIndex;

        ChangeState(transform.GetChild(stateIndex).gameObject);
    }

    [Server]
    public void ServerSpawnObject(GameObject obj)
    {
        NetworkServer.Spawn(obj);
    }
}

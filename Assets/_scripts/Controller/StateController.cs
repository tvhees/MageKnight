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
    public GameObject endOfTurn;

    public GameObject[] gameStates;

    [SyncVar(hook = "OnStateIndexChanged")]
    public int stateIndex;
    public GameObject gameState;
    public GameObject lastState;

    [Server]
    public void ChangeToState(GameConstants.GameState newState)
    {
        stateIndex = (int)newState;
        ChangeToState(gameStates[(int)newState]);
    }

    [Server]
    public void ChangeToState(GameObject newState)
    {
        stateIndex = newState.transform.GetSiblingIndex();
        StateChange(newState);
    }

    void StateChange(GameObject newState)
    {
        if (gameState == newState)
            return;

        if (gameState != null)
            gameState.SetActive(false);

        gameState = newState;
        gameState.SetActive(true);

        EventManager.stateChanged.Invoke(gameState.GetComponent<GameState>());
    }

    [Client]
    public void OnStateIndexChanged(int newIndex)
    {
        if (!isServer)
        {
            stateIndex = newIndex;

            StateChange(gameStates[newIndex]);
        }
    }

    [Server]
    public void SpawnObject(GameObject obj)
    {
        NetworkServer.Spawn(obj);
    }
}

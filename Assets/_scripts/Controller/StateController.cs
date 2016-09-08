using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class StateController : NetworkBehaviour {

    public GameObject characterSelect;
    public GameObject boardSetup;
    public GameObject tacticSelect;

    [SyncVar(hook = "OnStateIndexChanged")]
    public int stateIndex;
    public GameObject gameState;

    public override void OnStartClient()
    {
        base.OnStartClient();
        OnStateIndexChanged(stateIndex);
    }

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
    }

    [Client]
    public void OnStateIndexChanged(int newIndex)
    {
        stateIndex = newIndex;

        EventManager.debugMessage.Invoke(stateIndex.ToString());

        ChangeState(transform.GetChild(stateIndex).gameObject);
    }

    [Server]
    public void ServerSpawnObject(GameObject obj)
    {
        NetworkServer.Spawn(obj);
    }
}

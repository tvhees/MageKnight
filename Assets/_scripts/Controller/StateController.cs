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

    public void ChangeState(GameObject newState)
    {
        if (gameState == newState)
            return;

        if (gameState != null)
            gameState.SetActive(false);

        gameState = newState;
        gameState.SetActive(true);
    }

    public void OnStateIndexChanged(int newIndex)
    {
        stateIndex = newIndex;

        ChangeState(transform.GetChild(stateIndex).gameObject);
    }
}

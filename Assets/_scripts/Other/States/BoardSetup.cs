using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using Other.Data;
using Other.Factory;
using Other.Utility;

public class BoardSetup : NetworkBehaviour {

    public GameObject holderPrefab;

    public StateController stateController;
    public BoardView boardView;

    [ServerCallback]
    void OnEnable()
    {
        Debug.Log(gameObject.name);
        GameController.singleton.ServerCreateBoardFromRandomSeed();
        GameController.singleton.ServerSetPlayerPositions();
        stateController.ServerChangeToState(GameConstants.GameState.RoundSetup);
    }
}

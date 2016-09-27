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
    public BoardFactory boardFactory;
    public BoardView boardView;

    [ServerCallback]
    void OnEnable()
    {
        GameController.singleton.ServerCreateBoardFromRandomSeed();
        GameController.singleton.ServerSetPlayerPositions();
        stateController.ServerChangeState(stateController.tacticSelect);
    }
}

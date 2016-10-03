using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using Other.Data;
using Other.Factory;
using Other.Utility;

public class TurnSetup : NetworkBehaviour {

    public StateController stateController;

    [ServerCallback]
    void OnEnable()
    {
        GameController.singleton.currentPlayer.model.ResetMana();
        stateController.ChangeToState(stateController.startOfTurn);
    }
}

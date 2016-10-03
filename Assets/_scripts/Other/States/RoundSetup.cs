using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using Other.Data;
using Other.Factory;
using Other.Utility;

public class RoundSetup : NetworkBehaviour {

    public StateController stateController;

    [ServerCallback]
    void OnEnable()
    {
        Debug.Log(gameObject.name);
        GameController.singleton.RollAllDice();
        stateController.ChangeToState(GameConstants.GameState.TacticSelect);
    }
}

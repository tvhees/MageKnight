using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using Other.Data;
using Other.Factory;
using Other.Utility;

public class SelectionPhase : NetworkBehaviour {

    [ServerCallback]
    void OnEnable()
    {
        GameController.singleton.playerSelectionCounter = 0;
    }
}

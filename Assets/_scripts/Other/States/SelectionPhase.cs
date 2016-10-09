using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using Other.Data;
using Other.Factory;
using Other.Utility;

public class SelectionPhase : GameState {

    // Selection phases are ended by the GameController, so we don't call the next gamestate here
    [ServerCallback]
    protected override void OnEnable()
    {
        base.OnEnable();
        GameController.players.ClearNextOrder();
    }
}

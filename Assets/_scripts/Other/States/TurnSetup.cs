using UnityEngine;
using UnityEngine.Networking;


public class TurnSetup : GameState {

    [ServerCallback]
    protected override void OnEnable()
    {
        base.OnEnable();
        stateController.ChangeToState(stateController.startOfTurn);
    }
}

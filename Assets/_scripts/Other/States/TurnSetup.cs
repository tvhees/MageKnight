using UnityEngine;
using UnityEngine.Networking;


public class TurnSetup : GameState {

    [ServerCallback]
    protected override void OnEnable()
    {
        base.OnEnable();
        PlayerControl.current.model.ResetMana();
        stateController.ChangeToState(stateController.startOfTurn);
    }
}

using UnityEngine;
using UnityEngine.Networking;


public class TurnSetup : GameState {

    [ServerCallback]
    protected override void OnEnable()
    {
        base.OnEnable();
        GameController.currentPlayer.model.ResetMana();
        stateController.ChangeToState(stateController.startOfTurn);
    }
}

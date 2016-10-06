using UnityEngine;
using UnityEngine.Networking;


public class TurnSetup : GameState {

    [ServerCallback]
    protected override void OnEnable()
    {
        base.OnEnable();
        GameController.singleton.players.current.model.ResetMana();
        stateController.ChangeToState(stateController.startOfTurn);
    }
}

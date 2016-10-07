using UnityEngine;
using UnityEngine.Networking;

public class BoardSetup : GameState {

    [ServerCallback]
    protected override void OnEnable()
    {
        base.OnEnable();
        GameController.singleton.CreateGameFromRandomSeed();
        stateController.ChangeToState(GameConstants.GameState.RoundSetup);
    }
}

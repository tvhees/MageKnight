using UnityEngine.Networking;

public class RoundSetup : GameState
{
    [ServerCallback]
    protected override void OnEnable()
    {
        base.OnEnable();
        GameController.singleton.dice.RollAll();
        stateController.ChangeToState(GameConstants.GameState.TacticSelect);
    }
}
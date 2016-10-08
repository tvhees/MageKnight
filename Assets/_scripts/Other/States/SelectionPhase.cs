using UnityEngine.Networking;

public class SelectionPhase : GameState
{
    // Selection phases are ended by the GameController, so we don't call the next gamestate here
    [ServerCallback]
    protected override void OnEnable()
    {
        base.OnEnable();
        GameController.singleton.players.ClearNextOrder();
    }
}
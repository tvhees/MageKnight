using Other.Data;
using UnityEngine;
using UnityEngine.Networking;

public abstract class GameState : NetworkBehaviour
{
    [SerializeField]
    protected bool allowEffects;

    [SerializeField]
    protected StateController stateController;

    public Card[] tacticsTriggered = new Card[0];
    public bool AllowEffects { get { return allowEffects; } }

    protected virtual void OnEnable()
    {
        for (int i = 0; i < tacticsTriggered.Length; i++)
        {
            GameController.singleton.players.current.TriggerTactic(tacticsTriggered[i]);
        }
        //        Debug.Log(gameObject.name);
    }
}
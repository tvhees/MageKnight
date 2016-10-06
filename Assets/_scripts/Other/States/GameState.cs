using UnityEngine;
using UnityEngine.Networking;

public abstract class GameState : NetworkBehaviour
{
    [SerializeField]
    protected bool allowEffects;
    [SerializeField]
    protected StateController stateController;

    public bool AllowEffects { get { return allowEffects; } }

    protected virtual void OnEnable()
    {
        Debug.Log(gameObject.name);
    }
}
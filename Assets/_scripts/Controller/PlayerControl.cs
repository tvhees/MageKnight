using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class PlayerEvent : UnityEvent<PlayerControl> { }

public class PlayerControl : NetworkBehaviour 
{
    #region Events
    public static PlayerEvent Started = new PlayerEvent();
    public static PlayerEvent StartedLocal = new PlayerEvent();
    #endregion

    #region Network behaviour
    public override void OnStartClient()
    {
        base.OnStartClient();
        Started.Invoke(this);
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        StartedLocal.Invoke(this);
    }
    #endregion
}
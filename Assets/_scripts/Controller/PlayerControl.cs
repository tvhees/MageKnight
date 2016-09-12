using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using Other.Factory;
using Model;
using View;
using Other.Data;
using Other.Utility;

public class PlayerControl : NetworkBehaviour
{
    [SyncVar(hook = "OnPlayerIdChanged")]
    public int playerId;
    [SyncVar(hook = "OnPlayerNameChanged")]
    public string playerName;
    [SyncVar(hook = "OnColourChanged")]
    public Color colour;

    public bool isYourTurn { get { return GameController.singleton.currentPlayer == this; } }

    public CharacterView characterView;

    #region Initialisation
    public override void OnStartClient()
    {
        base.OnStartClient();
        StartCoroutine(WaitForSceneLoad());
    }

    IEnumerator WaitForSceneLoad()
    {
        while (GameController.singleton == null)
            yield return null;

        OnSceneLoaded();
    }

    void OnSceneLoaded()
    {
        OnPlayerIdChanged(playerId);
        OnPlayerNameChanged(playerName);
        OnColourChanged(colour);
    }

    // This fires BEFORE other NetworkIdentity objects are activated in the scene, but only in standalone builds
    // We use a coroutine to wait until scene objects are loaded, otherwise standalone builds are not initialised properly.
    public override void OnStartLocalPlayer()
    {
        StartCoroutine(WaitForLocalSceneLoad());
    }

    [Client]
    IEnumerator WaitForLocalSceneLoad()
    {
        while (GameController.singleton == null)
            yield return null;

        OnLocalSceneLoaded();
    }

    [Client]
    void OnLocalSceneLoaded()
    {
        EventManager.debugMessage.Invoke("Local Scene Loaded");
        GameController.singleton.localPlayer = this;
        CmdSetPlayerId(playerId);
        CmdAddToPlayerList();
    }
#endregion

#region Commands to server
    [Command]
    void CmdSetPlayerId(int newId)
    {
        playerId = newId;
    }

    [Command]
    void CmdAddToPlayerList()
    {
        GameController.singleton.ServerAddPlayer(this);
    }

    [Command]
    public void CmdSetCharacter(string name)
    {
        if (isYourTurn)
        {
            var character = CharacterDatabase.GetScriptableObject(name);
            colour = character.colour;

            GameController.singleton.ServerOnCharacterSelected(name);
        }
    }

    [Command]
    public void CmdSetTactic(string name)
    {
        if (isYourTurn)
        {
            GameController.singleton.ServerOnTacticSelected(name);
        }
    }

    [Command]
    public void CmdEndTurn()
    {
        if(isYourTurn)
            GameController.singleton.ServerNextPlayer();
    }
    #endregion

    #region Client methods
    [ClientRpc]
    public void RpcYourTurn(bool becameYourTurn)
    {
        float alpha = 2 / 6f;
        if (becameYourTurn)
        {
            alpha = 1f;
        }

        characterView.SetMaterialAlpha(alpha);
    }
    #endregion

    #region Hook methods
    [Client]
    void OnPlayerIdChanged(int newId)
    {
        playerId = newId;
    }

    [Client]
    void OnPlayerNameChanged(string playerName)
    {
        gameObject.name = playerName;
        // On initial load the GameController object won't be active in a build when this hook is called
        if (GameController.singleton != null)
            GameController.singleton.playerView.SetPlayerName(playerId, playerName);
    }

    [Client]
    void OnColourChanged(Color newColour)
    {
        colour = newColour;

        characterView.SetMaterialColour(newColour);

        if (GameController.singleton != null)
            GameController.singleton.playerView.SetPlayerColour(playerId, colour);
    }

#endregion
}
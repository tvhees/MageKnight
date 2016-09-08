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

    [SyncVar(hook = "OnColourChanged")]
    public Color colour;

    public bool isYourTurn { get { return GameController.singleton.currentPlayer == this; } }

    public TextMesh playerNameText;
    public CharacterView characterView;

    #region Initialisation
#if !UNITY_EDITOR
    void Start()
    {
        StartCoroutine(WaitForGameController());
    }

    IEnumerator WaitForGameController()
    {
        while (GameController.singleton == null)
            yield return null;

        Debug.Log("Ended Coroutine");

        OnStartPlayer();
    }
#endif

#if UNITY_EDITOR
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        OnStartPlayer();
    }
#endif

    void OnStartPlayer()
    {
        if (isLocalPlayer)
        {
            CmdSetPlayerId(playerId);
            GameController.singleton.localPlayer = this;
            CmdAddToPlayerList();
        }
    }

    void DebugEventMethod(Character input)
    {
        EventManager.debugMessage.Invoke("Event triggered: " + input.ToString());
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        OnPlayerIdChanged(playerId);
        OnColourChanged(colour);
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
            GameController.singleton.ServerNextPlayer();
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

        string playerNameString = "Player " + newId;
        if (isLocalPlayer)
            playerNameString += " (L)";

        gameObject.name = playerNameString;
        playerNameText.text = playerNameString;
    }

    [Client]
    void OnColourChanged(Color newColour)
    {
        colour = newColour;

        characterView.SetMaterialColour(newColour);
    }

#endregion
}
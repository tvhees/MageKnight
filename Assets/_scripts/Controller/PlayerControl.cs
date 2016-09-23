using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using Other.Factory;
using View;
using Other.Data;
using Other.Utility;

public class PlayerControl : NetworkBehaviour
{
    [SyncVar(hook = "OnPlayerIdChanged")]
    public int playerId;
    [SyncVar(hook = "OnPlayerNameChanged")]
    public string playerName;
    [SyncVar(hook = "OnCharacterNameChanged")]
    public string characterName;
    [SyncVar(hook = "OnColourChanged")]
    public Color colour;

    public bool isYourTurn { get { return GameController.singleton.currentPlayer == this; } }

    public Player model;
    public PlayerView view;
    public Character character;

    public Camera playerCamera;
    public TurnOrderDisplay turnOrderDisplay;
    public CharacterView characterView;
    public NetworkIdentity networkIdentity;

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
        base.OnStartLocalPlayer();
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
        GameController.singleton.localPlayer = this;
        playerCamera.enabled = true;
        turnOrderDisplay.toggle.isOn = true;
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
            characterName = name;
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
    void OnPlayerNameChanged(string newName)
    {
        playerName = newName;
        gameObject.name = playerName;

        if (HasTurnOrderDisplay())
            turnOrderDisplay.SetPlayerName(playerName);
    }

    [Client]
    void OnCharacterNameChanged(string newName)
    {
        character = CharacterDatabase.GetScriptableObject(newName);

        if (HasTurnOrderDisplay())
            turnOrderDisplay.SetCharacterName(newName);
    }

    [Client]
    void OnColourChanged(Color newColour)
    {
        colour = newColour;

        characterView.SetMaterialColour(newColour);

        if (HasTurnOrderDisplay())
            turnOrderDisplay.SetPlayerColour(colour);
    }

    [Client]
    bool HasTurnOrderDisplay()
    {
        // On initial load the GameController object won't be active in a build when this hook is called
        if (GameController.singleton == null)
            return false;

        if (turnOrderDisplay == null)
        {
            turnOrderDisplay = GameController.singleton.playerView.GetTurnOrderDisplay(playerId);
            turnOrderDisplay.AssignToPlayer(this);
        }

        return true;
    }

    [Client]
    public void Show()
    {
        playerCamera.enabled = true;
        view.Show();
    }

    [Client]
    public void Hide()
    {
        playerCamera.enabled = false;
        view.Hide();
    }

    [ClientRpc]
    public void RpcMoveToIndexInTurnOrder(int index)
    {
        turnOrderDisplay.transform.SetSiblingIndex(index);
    }
    #endregion

    #region Card Management
    [Server]
    public void CreateModel(Cards cards)
    {
        model = new Player(character, cards);
        foreach (var cardId in model.deck)
            view.RpcAddCardToDeck(cardId);
    }

    [Server]
    public void DrawCards(int numberToDraw)
    {
        model.DrawCards(numberToDraw);
        view.RpcDrawCards(numberToDraw);
    }

    [Server]
    public void ServerMoveCardToDeck(GameObject card)
    {
        card.GetComponent<CardView>().MoveToNewParent(view.deck.transform);
    }

    [Server]
    public void AssignChosenTactic(Cards cards, Card tactic)
    {
        view.RpcOnTacticChosen(cards.GetTacticId(tactic.number));
    }
    #endregion
}
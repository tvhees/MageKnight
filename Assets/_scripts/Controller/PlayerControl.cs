using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Events;
using UnityEngine.Assertions;
using System.Collections;
using System.Collections.Generic;
using Other.Factory;
using View;
using Other.Data;
using Other.Utility;
using Commands;

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
    [SyncVar(hook = "OnHexChanged")]
    public HexId currentHex;

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
        OnHexChanged(currentHex);
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
        turnOrderDisplay.Select(true);
        CmdSetPlayerId(playerId);
        CmdAddToPlayerList();
    }
    #endregion

    #region Server methods

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

    [Command]
    public void CmdUndo()
    {
        if (GameController.singleton.currentPlayer == this)
            GameController.singleton.commandStack.UndoLastCommand();
    }

    [Command]
    public void CmdPlayEffect(CardId cardId)
    {
        Assert.IsTrue(model.ListContainsCard(cardId, model.hand), "Card played is not in hand on the server");

        Card card = CardDatabase.GetScriptableObject(cardId.name);
        Command effect = card.GetAutomaticEffect();
        if (effect == null)
            return;

        effect = Instantiate(effect);
        effect.SetInformation(new GameData(player: this, cardId: cardId));
        GameController.singleton.commandStack.RunCommand(effect);
    }

    [Command]
    public void CmdMoveToHex(HexId newHex)
    {
        var moveToHex = Instantiate(CommandDatabase.GetScriptableObject("MoveToHex"));
        moveToHex.SetInformation(new GameData(player: this, hexId: newHex));
        GameController.singleton.commandStack.RunCommand(moveToHex);
    }
    #endregion

    #region Turn order and UI view
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

    [Client]
    bool HasTurnOrderDisplay()
    {
        // On initial load the GameController object won't be active in a build when this hook is called
        if (GameController.singleton == null)
            return false;

        if (turnOrderDisplay == null)
        {
            turnOrderDisplay = GameController.singleton.sharedView.GetTurnOrderDisplay(playerId);
            turnOrderDisplay.AssignToPlayer(this);
        }

        return true;
    }

    [Client]
    public void ShowUi()
    {
        playerCamera.enabled = true;
        view.Show();
    }

    [Client]
    public void HideUi()
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
    #endregion

    #region Mana
    [Command]
    public void CmdDieToggled(bool selected)
    {
        if (selected)
            model.diceAllowed--;
        else
            model.diceAllowed++;

        if (model.diceAllowed <= 0)
            RpcToggleDiceInteractivity(false);
        else
            RpcToggleDiceInteractivity(true);
    }

    [ClientRpc]
    public void RpcToggleDiceInteractivity(bool interactible)
    {
        if(isLocalPlayer)
            GameController.singleton.sharedView.ToggleDice(interactible);
    }

    [Command]
    public void CmdAddMana(GameConstants.ManaType manaType)
    {
        model.AddMana(manaType);
    }

    [Command]
    public void CmdRemoveMana(GameConstants.ManaType manaType)
    {
        model.AddMana(manaType, subtract: true);
    }

    [Command]
    public void CmdAddCrystal(GameConstants.ManaType manaType)
    {
        model.crystals[(int)manaType]++;
    }

    [Command]
    public void CmdRemoveCrystal(GameConstants.ManaType manaType)
    {
        model.crystals[(int)manaType]--;
    }
    #endregion

    #region Card Management
    [Server]
    public void CreateModel(Cards cards)
    {
        model = new Player(character, cards);
        for (int i = 0; i < model.deck.Count; i++)
        {
            CardId card = model.deck[i];
            view.RpcAddCardToDeck(card);
        }
    }

    [Server]
    public void DrawCards(int numberToDraw)
    {
        model.DrawCards(numberToDraw);
        view.RpcDrawCards(numberToDraw);
    }

    [Server]
    public void ServerMoveCard(CardId card, GameConstants.Collection to)
    {
        switch (to)
        {
            case GameConstants.Collection.Hand:
                ServerMoveCardToHand(card);
                break;
            case GameConstants.Collection.Deck:
                break;
            case GameConstants.Collection.Discard:
                ServerMoveCardToDiscard(card);
                break;
            case GameConstants.Collection.Units:
                break;
        }
    }

    [Server]
    public void ServerMoveCardToHand(CardId card)
    {
        model.MoveCardToHand(card);
        view.RpcMoveCardToHand(card);
    }

    [Server]
    public void ServerMoveCardToDiscard(CardId card)
    {
        model.MoveCardToDiscard(card);
        view.RpcMoveCardToDiscard(card);
    }

    [Server]
    public void ServerMoveCardToDeck(CardId card)
    {
        model.MoveCardToDeck(card);
        view.RpcMoveCardToDeck(card);
    }

    [Server]
    public void ServerMoveCardToUnits(CardId card)
    {
        model.MoveCardToUnits(card);
        view.RpcMoveCardToUnits(card);
    }

    [Server]
    public void AssignChosenTactic(Cards cards, Card tactic)
    {
        view.RpcOnTacticChosen(cards.GetTacticId(tactic.number));
    }
    #endregion

    #region Movement
    [Server]
    public void ServerAddMovement(int value)
    {
        model.movement += value;
        view.RpcUpdateMovement(model.movement);
    }

    [Server]
    public void ServerAddInfluence(int value)
    {
        model.influence += value;
        view.RpcUpdateInfluence(model.influence);
    }

    public bool CanMoveToHex(HexId newHex)
    {
        if (Vector3.SqrMagnitude(currentHex.position - newHex.position) > GameConstants.sqrTileDistance)
        {
            Debug.Log("Too far away");
            return false;
        }

        if (!newHex.isTraversable)
        {
            Debug.Log("Impassable terrain");
            return false;
        }

        if (model.movement < newHex.movementCost)
        {
            Debug.Log(model.movement + " movement, need " + newHex.movementCost);
            return false;
        }

        return true;
    }

    public void OnHexChanged(HexId newHex)
    {
        currentHex = newHex;
        characterView.MoveToHex(newHex);
    }
    #endregion

    void Update()
    {
        if (!isLocalPlayer)
            return;

        if (Input.GetKeyDown(KeyCode.U) || Input.GetKeyDown(KeyCode.Mouse1))
            CmdUndo();
    }
}
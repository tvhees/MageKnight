using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Networking;
using System.Collections;
using View;
using Other.Data;
using Other.Utility;
using Commands;
using RSG;
using System;

public class PlayerControl : NetworkBehaviour
{
    #region Attributes

    public static PlayerControl current;
    public static PlayerControl local;

    [SyncVar]
    public int playerId;
    [SyncVar(hook = "OnPlayerNameChanged")]
    public string playerName;
    [SyncVar(hook = "OnCharacterNameChanged")]
    public string characterName;
    [SyncVar(hook = "OnColourChanged")]
    public Color colour;
    [SyncVar(hook = "OnHexChanged")]
    HexId currentHex;
    [SyncVar]
    public CommandResult commandSuccess;
    [SyncVar]
    public bool commandRunning;

    #endregion Attributes

    #region References

    public Player model;
    public PlayerView view;
    Character character;

    [SerializeField] Camera playerCamera;
    [SerializeField] TurnOrderDisplay turnOrderDisplay;
    [SerializeField] CharacterView characterView;

    #endregion References

    #region Properties

    public bool IsCurrentPlayer { get { return current == this; } }
    bool HasTurnOrderDisplay
    {
        get
        {
            // On initial load the GameController object won't be active in a build when this is called
            if (GameController.singleton == null) return false;
            if (turnOrderDisplay == null)
            {
                turnOrderDisplay = GameController.singleton.sharedView.GetTurnOrderDisplay(playerId);
                turnOrderDisplay.AssignToPlayer(this);
            }
            Assert.IsNotNull(turnOrderDisplay);
            return true;
        }
    }
    public bool CanDrawCards { get { return model.deck.Count > 0; } }
    public HexId CurrentHex { get { return currentHex; } }

    #endregion Properties

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

        OnClientSceneLoaded();
    }

    // Ensures this player's variables are correctly set on subsequently connected clients
    void OnClientSceneLoaded()
    {
        OnPlayerNameChanged(playerName);
        OnColourChanged(colour);
        OnHexChanged(currentHex);
    }

    // Fires BEFORE other NetworkIdentity objects are activated in the scene, but only in standalone builds
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        StartCoroutine(WaitForLocalSceneLoad());
    }

    // Coroutine waits until scene objects are loaded, otherwise standalone builds are not initialised properly.
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
        local = this;
        playerCamera.enabled = true;
        turnOrderDisplay.Select(true);
        CmdAddToPlayerList();
    }

    [Command]
    void CmdAddToPlayerList()
    {
        GameController.players.ServerAdd(this);
    }
    #endregion

    #region Hook methods
    [Client]
    void OnPlayerNameChanged(string newName)
    {
        playerName = newName;
        gameObject.name = playerName;

        if (HasTurnOrderDisplay)
            turnOrderDisplay.SetPlayerName(playerName);
    }

    [Client]
    void OnCharacterNameChanged(string newName)
    {
        character = CharacterDatabase.GetScriptableObject(newName);

        if (HasTurnOrderDisplay)
            turnOrderDisplay.SetCharacterName(newName);
    }

    [Client]
    void OnColourChanged(Color newColour)
    {
        colour = newColour;

        characterView.SetMaterialColour(newColour);

        if (HasTurnOrderDisplay)
            turnOrderDisplay.SetPlayerColour(colour);
    }
    #endregion

    #region UI responses
    [Command]
    public void CmdSetCharacter(string name)
    {
        characterName = name;
        colour = character.colour;

        GameController.singleton.OnCharacterSelected(name);
    }

    [Command]
    public void CmdSetTactic(string name)
    {
        GameController.singleton.OnTacticSelected(name);
    }
    [Command]
    public void CmdUndo()
    {
        if (IsCurrentPlayer)
            GameController.singleton.commandStack.UndoLastCommand();
    }

    [Command]
    public void CmdEndTurn()
    {
        GameController.singleton.EndTurn();
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
    #endregion UI responses

    #region Turn order and UI view
    [Server]
    public void ServerEndTurn()
    {
        model.ServerEndTurn(this);
        view.RpcUpdateInfluence(model.influence);
        view.RpcUpdateMovement(model.movement);
    }

    [ClientRpc]
    public void RpcNewTurn(bool thisPlayerTurn)
    {
        if (thisPlayerTurn)
            current = this;

        var alpha = thisPlayerTurn ? 1f : 2 / 5f;

        characterView.SetMaterialAlpha(alpha);

        if (GameController.singleton.sharedView != null)
        {
            GameController.singleton.sharedView.TogglePlayerHighlight(playerId, thisPlayerTurn);
            GameController.singleton.sharedView.ToggleDice(thisPlayerTurn);
        }
    }
    [ClientRpc]
    public void RpcMoveToIndexInTurnOrder(int index)
    {
        turnOrderDisplay.transform.SetSiblingIndex(index);
    }
    #endregion

    #region Mana
    [Command]
    public void CmdDieToggled(ManaId manaId)
    {
        model.DieToggled(manaId);
        GameController.singleton.dice.SetDieValue(manaId);

        if (model.CanUseDice)
            RpcToggleDiceInteractivity(true);
        else
            RpcToggleDiceInteractivity(false);
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
    public void ServerCreateModel(Cards cards)
    {
        model = new Player(character, cards);
        view.RpcUpdateReputation(model.reputation);
        view.RpcUpdateFame(model.fame);
        for (int i = 0; i < model.deck.Count; i++)
        {
            var card = model.deck[i];
            view.RpcAddNewCardToDeck(card);
        }
    }

    [Server]
    public void ServerDrawCards(int numberToDraw)
    {
        for (int i = 0; i < numberToDraw; i++)
        {
            if (!CanDrawCards)
                break;

            ServerMoveCard(model.deck.GetFirst(), GameConstants.Location.Hand);
            GameController.singleton.commandStack.ClearCommandList();
        }
    }

    [Server]
    public void ServerRefillHand()
    {
        var numberToDraw = Mathf.Max(model.handSize - model.hand.Count);
        ServerDrawCards(numberToDraw);
    }

    [Server]
    public void ServerMoveCard(CardId card, GameConstants.Location toLocation)
    {
        switch (toLocation)
        {
            case GameConstants.Location.Hand:
                ServerMoveCardToHand(card);
                break;
            case GameConstants.Location.Deck:
                ServerMoveCardToDeck(card);
                break;
            case GameConstants.Location.Discard:
                ServerMoveCardToDiscard(card);
                break;
            case GameConstants.Location.Units:
                ServerMoveCardToUnits(card);
                break;
            case GameConstants.Location.Play:
                ServerMoveCardToPlay(card);
                break;
            case GameConstants.Location.Limbo:
                ServerMoveCardToLimbo(card);
                break;
        }
    }

    [Server]
    public void ServerMoveCardToLimbo(CardId card)
    {
        model.MoveCardToLimbo(card);
        view.RpcMoveCardToLimbo(card);
    }

    [Server]
    public void ServerMoveCardToHand(CardId card)
    {
        model.MoveCardToHand(card);
        view.RpcMoveCardToHand(card);
    }

    [Server]
    public void ServerMoveCardToPlay(CardId card)
    {
        model.MoveCardToPlay(card);
        view.RpcMoveCardToPlay(card);
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
        var tacticId = cards.GetTacticId(tactic.number);
        model.tacticId = tacticId;
        model.tacticIsActive = true;
        view.RpcOnTacticChosen(tacticId);
    }

    [Server]
    public void TriggerTactic(Card tactic)
    {
        if (!model.tacticIsActive)
            return;

        if (model.tacticId.name == tactic.name)
        {
            var tacticCommand = tactic.GetEffect(tactic.AutomaticIndex);

            tacticCommand.SetInformation(new GameData(this));
            GameController.singleton.commandStack.RunCommand(tacticCommand);

            model.tacticIsActive = tactic.IsRepeatable;
        }
    }

    [Command]
    public void CmdPlayCard(CardId cardId)
    {
        var startLocation = cardId.location;
        var card = CardDatabase.GetScriptableObject(cardId.name);
        var gameData = new GameData(this, cardId);
        ServerMoveCard(cardId, GameConstants.Location.Limbo);

        PlayCardEffect(card, card.AutomaticIndex, gameData);
    }

    // This recursively plays effects from the card's definition until either one succeeds or they all fail.
    void PlayCardEffect(Card card, int index, GameData gameData)
    {
        // If there's no more commands to use they've all failed and we want to return the card to where it came from
        if (!card.HasEffect(index))
        {
            ServerMoveCard(gameData.cardId, gameData.cardId.location);
            return;
        }
        var effect = card.GetEffect(index);
        Assert.IsNotNull(effect);
        effect.SetInformation(gameData);
        GameController.singleton.commandStack.RunCommand(effect, () => PlayCardEffect(card, index + 1, gameData));
    }
    #endregion

    #region Movement
    [Server]
    public void ServerAddMovement(int value)
    {
        model.movement += value;
        view.RpcUpdateMovement(model.movement);
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

    [Command]
    public void CmdMoveToHex(HexId newHex)
    {
        var moveToHex = Instantiate(CommandDatabase.GetScriptableObject("MoveToHex"));
        moveToHex.SetInformation(new GameData(player: this, hexId: newHex));
        GameController.singleton.commandStack.RunCommand(moveToHex);
    }

    public void OnHexChanged(HexId newHex)
    {
        currentHex = newHex;
        characterView.MoveToHex(newHex);
    }

    #endregion

    #region Influence

    [Server]
    public void ServerAddInfluence(int value)
    {
        model.influence += value;
        view.RpcUpdateInfluence(model.influence);
    }

    [Server]
    public void ServerAddReputation(int value)
    {
        model.reputation += value;
        view.RpcUpdateReputation(model.reputation);
    }

    #endregion Influence

    #region Fame and levels

    [Server]
    public void ServerAddFame(int value)
    {
        model.fame += value;
        view.RpcUpdateFame(model.fame);
    }

    #endregion Fame and levels
}
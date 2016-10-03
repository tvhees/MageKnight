using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using View;
using Other.Data;
using Other.Utility;
using Other.Factory;
using Commands;
using Prototype.NetworkLobby;

public class GameController : NetworkBehaviour
{
    public static GameController singleton;

    #region Model
    public Board board;
    public Cards cards;
    public ManaPool mana;
    public StateController stateController;
    public CommandStack commandStack;
    #endregion

    #region View
    public BoardView boardView;
    public SharedView sharedView;
    public ToolTip toolTip;
    public GameObject zoomDisplayPanel;
    #endregion

    #region Settings
    public int randomSeed;
    public Scenario scenario { get {
            if (players.Count <= 1)
                return ScenarioDatabase.GetScriptableObject("Solo Conquest");
            else
                return ScenarioDatabase.GetScriptableObject("Full Conquest");
        } }
    #endregion

    #region Player variables
    public GameConstants.TerrainCosts movementCosts = new GameConstants.TerrainCosts(true);
    public int nextPlayerIndex;
    public int expectedPlayers;
    public int connectedClients = 0;

    public PlayerControl localPlayer;
    public List<PlayerControl> players = new List<PlayerControl>();
    public PlayerControl[] nextTurnOrder;
    public PlayerControl currentPlayer;
    #endregion

    public CardFactory cardFactory;
    public DebugPanel debugPanel;

    #region Initialisation
    void Awake()
    {
        singleton = this;
        AddEventListeners();
    }

    void AddEventListeners()
    {
        EventManager.characterSelected.AddListener(UiSelectCharacter);
        EventManager.tacticSelected.AddListener(UiSelectTactic);
        EventManager.endTurn.AddListener(UiEndTurn);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        expectedPlayers = ServerCalculateExpectedPlayers();
        StartCoroutine(ServerWaitForConnections());
    }

    [Server]
    IEnumerator ServerWaitForConnections()
    {
        while (players.Count < expectedPlayers)
            yield return null;

        StartGame();
    }

    [Server]
    public void AddPlayerToGame(PlayerControl player)
    {
        players.Add(player);
    }

    [Server]
    int ServerCalculateExpectedPlayers()
    {
        int i = 0;
        foreach (NetworkLobbyPlayer player in LobbyManager.s_Singleton.lobbySlots)
        {
            if(player != null)
                i++;
        }
        return i;
    }

    [Server]
    public void StartGame()
    {
        RandomiseTurnOrder();
        stateController.ChangeToState(GameConstants.GameState.CharacterSelect);
    }
    #endregion

    #region Game setup
    [Server]
    public void CreateGameFromRandomSeed()
    {
        // If we haven't specified a seed its value will be 0 and we should create a new one
        if (randomSeed == 0)
            randomSeed = System.Environment.TickCount;
        Random.InitState(randomSeed);

        board = new Board(scenario, players.ToArray(), boardView);
        cards = new Cards(scenario, players.ToArray());
        mana = new ManaPool(players.Count);
        sharedView.RpcEnableDice(mana.dice.Length);
    }
    #endregion

    #region SelectionPhases
    void UiSelectCharacter(string name)
    {
        localPlayer.CmdSetCharacter(name);
    }

    [Server]
    public void OnCharacterSelected(string name)
    {
        sharedView.RpcDisableButton(name);

        // Tactics selection phase proceeds in reverse order of character selection
        int newIndex = players.Count - nextPlayerIndex;
        nextTurnOrder[players.Count - nextPlayerIndex] = currentPlayer;

        if (nextPlayerIndex >= players.Count)
        {
            ServerSetNewTurnOrder();
            stateController.ChangeToState(GameConstants.GameState.BoardSetup);
        }
        else
            NextPlayer();
    }

    void UiSelectTactic(string name)
    {
        localPlayer.CmdSetTactic(name);
    }

    [Server]
    public void ServerOnTacticSelected(string name)
    {
        sharedView.RpcDisableButton(name);

        var tactic = CardDatabase.GetScriptableObject(name);
        nextTurnOrder[tactic.number] = currentPlayer;
        currentPlayer.AssignChosenTactic(cards, tactic);

        if (nextPlayerIndex >= players.Count)
        {
            ServerSetNewTurnOrder();
            stateController.ChangeToState(GameConstants.GameState.TurnSetup);
        }
        else
            NextPlayer();
    }
    #endregion

    #region Player management
    [Server]
    public void RandomiseTurnOrder()
    {
        nextTurnOrder = players.ToArray();
        nextTurnOrder.Shuffle();
        ServerSetNewTurnOrder();
    }

    [Server]
    public void NextPlayer()
    {
        if (currentPlayer != null)
        {
            currentPlayer.RpcYourTurn(false);
            sharedView.RpcStopHighlightingPlayer(currentPlayer.playerId);
        }

        if (nextPlayerIndex >= players.Count)
            nextPlayerIndex = 0;
        currentPlayer = players[nextPlayerIndex];
        currentPlayer.RpcYourTurn(true);
        if (sharedView != null)
        {
            sharedView.RpcHighlightPlayer(currentPlayer.playerId);
        }

        // Wrap currentPlayerIndex to 0 because Mathf.Repeat doesn't take integers
        nextPlayerIndex++;
    }

    [Server]
    public void ServerSetNewTurnOrder()
    {
        players.Clear();
        for (int i = 0; i < nextTurnOrder.Length; i++)
        {
            if (nextTurnOrder[i] == null)
                continue;

            nextTurnOrder[i].RpcMoveToIndexInTurnOrder(players.Count);
            players.Add(nextTurnOrder[i]);
        }

        nextPlayerIndex = 0;
        NextPlayer();
    }

    void UiEndTurn()
    {
        localPlayer.CmdEndTurn();
    }
    #endregion

    #region Dice and mana management
    [Server]
    public void RollAllDice()
    {
        for (int i = 0; i < mana.dice.Length; i++)
        {
            RollDie(i);
        }

        if (!mana.HasEnoughBasicMana())
            RollAllDice();
    }

    [Server]
    public void RollDie(int i)
    {
        GameConstants.ManaType manaColour = GameConstants.manaColours[Random.Range(0, GameConstants.manaColours.Length)];
        mana.dice[i].colour = manaColour;
        sharedView.RpcSetDiceColour(new ManaId(i, manaColour));
    }

    public void UiDieToggled(ManaId manaId)
    {
        localPlayer.CmdDieToggled(manaId.selected);

        if (manaId.selected)
            localPlayer.CmdAddMana(manaId.colour);
        else
            localPlayer.CmdRemoveMana(manaId.colour);
    }
    #endregion

    #region Command and effect management
    [Server]
    public void EnableUndo(bool enable)
    {
        currentPlayer.view.RpcEnableUndo(enable);
    }

    public void UiPlayEffect(CardId cardId)
    {
        localPlayer.CmdPlayEffect(cardId);
    }
    #endregion
}
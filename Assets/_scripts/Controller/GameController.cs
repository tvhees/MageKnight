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

    public int randomSeed;
    public Board board;
    public Cards cards;

    public Scenario scenario { get {
            if (players.Count <= 1)
                return ScenarioDatabase.GetScriptableObject("Solo Conquest");
            else
                return ScenarioDatabase.GetScriptableObject("Full Conquest");
        } }

    #region Model variables
    public GameConstants.TerrainCosts movementCosts;
    #endregion

    #region Syncvars
    [SyncVar]
    public int startPlayerIndex;
    [SyncVar]
    public int nextPlayerIndex;
    [SyncVar]
    public int expectedPlayers;
    [SyncVar]
    public int numberOfPlayers;
    #endregion

    #region References
    public PlayerControl localPlayer;
    public List<PlayerControl> players = new List<PlayerControl>();
    public PlayerControl[] nextTurnOrder;
    public PlayerControl currentPlayer;

    public BoardView boardView;
    public ToolTip toolTip;

    public int connectedClients = 0;

    public StateController stateController;
    public CommandStack commandStack;
    public CardFactory cardFactory;

    public SharedView sharedView;
    public GameObject displayPanel;
    public DebugPanel debugPanel;
    #endregion

    void Awake()
    {
        singleton = this;
        commandStack = ScriptableObject.CreateInstance<CommandStack>();
        AddEventListeners();
    }

    void AddEventListeners()
    {
        EventManager.characterSelected.AddListener(UiSelectCharacter);
        EventManager.tacticSelected.AddListener(UiSelectTactic);
        EventManager.endTurn.AddListener(UiEndTurn);
    }

    #region UnetCallbacks
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

        ServerOnAllConnected();
    }

    [Server]
    void ServerOnAllConnected()
    {
        ServerStartGame();
        stateController.ServerChangeState(stateController.characterSelect);
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
    #endregion

    #region Server methods
    [Server]
    public void ServerAddPlayer(PlayerControl player)
    {
        players.Add(player);
        numberOfPlayers = players.Count;
    }

    [Server]
    public void ServerStartGame()
    {
        ServerRandomiseTurnOrder();
    }

    [Server]
    public void ServerRandomiseTurnOrder()
    {
        nextTurnOrder = players.ToArray();
        nextTurnOrder.Shuffle();
        ServerSetNewTurnOrder();
    }

    [Server]
    public void ServerOnCharacterSelected(string name)
    {
        sharedView.RpcDisableButton(name);
        
        // Tactics selection phase proceeds in reverse order of character selection
        nextTurnOrder[players.Count - nextPlayerIndex] = currentPlayer;

        if (nextPlayerIndex >= players.Count)
            stateController.ServerChangeState(stateController.boardSetup);
        else
            ServerNextPlayer();
    }

    [Server]
    public void ServerCreateBoardFromRandomSeed()
    {
        // If we haven't specified a seed its value will be 0 and we should create a new one
        if (randomSeed == 0)
            randomSeed = System.Environment.TickCount;
        Random.seed = randomSeed;
        board = new Board(scenario, numberOfPlayers, boardView);
        cards = new Cards(scenario, players.ToArray());
    }

    [Server]
    public void ServerOnTacticSelected(string name)
    {
        sharedView.RpcDisableButton(name);

        var tactic = CardDatabase.GetScriptableObject(name);
        nextTurnOrder[tactic.number] = currentPlayer;
        currentPlayer.AssignChosenTactic(cards, tactic);

        if (nextPlayerIndex >= players.Count)
            stateController.ServerChangeState(stateController.startOfRound);
        else
            ServerNextPlayer();
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
        ServerNextPlayer();
    }

    [Server]
    public void ServerNextPlayer()
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
    #endregion

    #region UI methods
    void UiSelectCharacter(string name)
    {
        localPlayer.CmdSetCharacter(name);
    }

    void UiSelectTactic(string name)
    {
        localPlayer.CmdSetTactic(name);
    }

    void UiEndTurn()
    {
        localPlayer.CmdEndTurn();
    }
#endregion
}
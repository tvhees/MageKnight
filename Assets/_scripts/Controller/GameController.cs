using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using View;
using Other.Data;
using Other.Utility;
using Other.Factory;
using Prototype.NetworkLobby;

public class GameController : NetworkBehaviour
{
    public static GameController singleton;

    public Scenario scenario { get {
            if (players.Count <= 1)
                return ScenarioDatabase.GetScriptableObject("Solo Conquest");
            else
                return ScenarioDatabase.GetScriptableObject("Full Conquest");
        } }

    #region References
    public PlayerControl localPlayer;
    public List<PlayerControl> players = new List<PlayerControl>();

    public int startPlayerIndex;
    public int currentPlayerIndex = -1;
    public int expectedPlayers;
    public PlayerControl currentPlayer;

    public int playerSelectionCounter = 0;

    public StateController stateController;
    public CommandStack commandStack;
    #endregion

    public PlayerView playerView;
    public DebugPanel debugPanel;

    void Awake()
    {
        singleton = this;
        commandStack = ScriptableObject.CreateInstance<CommandStack>();
        AddEventListeners();
    }

    void AddEventListeners()
    {
        EventManager.characterSelected.AddListener(UiSelectCharacter);
        EventManager.endTurn.AddListener(UiEndTurn);
    }

    #region UnetCallbacks
    public override void OnStartServer()
    {
        base.OnStartServer();
    }

    [Server]
    void ServerCalculateExpectedPlayers()
    {
        int i = 0;
        foreach (NetworkLobbyPlayer player in LobbyManager.s_Singleton.lobbySlots)
        {
            if(player != null)
                i++;
        }
        expectedPlayers = i;
    }
#endregion

#region Server methods
    [Server]
    public void ServerAddPlayer(PlayerControl player)
    {
        players.Add(player);
        ServerCalculateExpectedPlayers();

        if (players.Count == expectedPlayers)
        {
            ServerStartGame();
            stateController.ServerChangeState(stateController.characterSelect);
        }
    }

    [Server]
    public void ServerStartGame()
    {
        currentPlayerIndex = Random.Range(0, players.Count);
        ServerNextPlayer();
    }

    [Server]
    public void ServerOnCharacterSelected(string name)
    {
        playerView.RpcDisableCharacterButton(name);
        playerSelectionCounter++;
        if (playerSelectionCounter >= players.Count)
            stateController.ServerChangeState(stateController.boardSetup);
    }

    [Server]
    public void ServerNextPlayer()
    {
        if (currentPlayer != null)
            currentPlayer.RpcYourTurn(false);

        // Wrap currentPlayerIndex to 0 Mathf.Repeat because it doesn't take integers
        currentPlayerIndex++;
        if (currentPlayerIndex >= players.Count)
            currentPlayerIndex = 0;

        currentPlayer = players[currentPlayerIndex];

        currentPlayer.RpcYourTurn(true);
    }
    #endregion

    #region UI methods
    public void UiSelectCharacter(string name)
    {
        localPlayer.CmdSetCharacter(name);
    }

    public void UiEndTurn()
    {
        localPlayer.CmdEndTurn();
    }
#endregion
}
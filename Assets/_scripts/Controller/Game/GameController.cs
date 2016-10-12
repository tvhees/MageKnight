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

    #region Controller
    public static GamePlayers players { get; private set; }
    public GameDice dice { get; private set; }
    #endregion

    #region Model
    public Board board;
    public Cards cards;
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
    public Scenario scenario {
        get {
            if (players.Connected <= 1)
                return ScenarioDatabase.GetScriptableObject("Solo Conquest");
            else
                return ScenarioDatabase.GetScriptableObject("Full Conquest"); }
    }
    #endregion

    public GameConstants.TerrainCosts movementCosts = new GameConstants.TerrainCosts(true);
    public CardFactory cardFactory;
    public DebugPanel debugPanel;

    #region Initialisation
    void Awake()
    {
        singleton = this;
        players = new GamePlayers();
        dice = GetComponent<GameDice>();
        AddEventListeners();
    }

    void AddEventListeners()
    {
        EventManager.characterSelected.AddListener(UiSelectCharacter);
        EventManager.tacticSelected.AddListener(UiSelectTactic);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        StartCoroutine(ServerWaitForConnections());
    }

    [Server]
    IEnumerator ServerWaitForConnections()
    {
        while (!players.AllConnected)
            yield return null;

        StartGame();
    }

    [Server]
    int CalculateExpectedPlayers()
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
        players.ServerRandomiseOrder();
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

        board = new Board(scenario, players, boardView);
        cards = new Cards(players);
        var mana = new ManaPool(players);

        dice.Enable(mana, sharedView);
    }
    #endregion

    #region SelectionPhases
    void UiSelectCharacter(string characterName)
    {
        if(PlayerControl.local.IsCurrentPlayer) PlayerControl.local.CmdSetCharacter(characterName);
    }

    [Server]
    public void OnCharacterSelected(string name)
    {
        var endRound = players.OnLastForRound;

        sharedView.RpcDisableButton(name);
        players.ServerAssignCharacter();
        
        if(endRound)
            stateController.ChangeToState(GameConstants.GameState.BoardSetup);
    }

    void UiSelectTactic(string tacticName)
    {
        if(PlayerControl.local.IsCurrentPlayer) PlayerControl.local.CmdSetTactic(tacticName);
    }

    [Server]
    public void OnTacticSelected(string name)
    {
        var endRound = players.OnLastForRound;

        sharedView.RpcDisableButton(name);
        var tactic = CardDatabase.GetScriptableObject(name);
        players.ServerAssignTactic(cards, tactic);

        if (endRound)
            stateController.ChangeToState(GameConstants.GameState.TurnSetup);
    }
    #endregion

    #region Mana
    [Server]
    public ManaId PlayManaSource(GameConstants.ManaType colour)
    {
        ManaId source;
        source = dice.GetSelected(colour);
        if (source.index < 0)
        {
            Debug.Log("No selected die of colour " + colour.ToString());
            return source;
        }
        source.selected = false;
        dice.usedDice.Add(source);
        sharedView.RpcMoveDieToPlay(source);
        return source;
    }

    [Server]
    public void ReturnManaSource(ManaId source)
    {
        sharedView.RpcMoveDieToPool(source);
    }

    [Server]
    public void ReturnAllDice()
    {
        for (int i = 0; i < dice.usedDice.Count; i++)
        {
            var die = dice.usedDice[i];
            dice.RollDie(die.index);
            sharedView.RpcMoveDieToPool(die);
        }

        dice.usedDice.Clear();
    }
    #endregion

    public void EndTurn()
    {
        stateController.ChangeToState(stateController.endOfTurn);
        players.ServerEndTurn();
        ReturnAllDice();
        stateController.ChangeToState(stateController.turnSetup);
    }
}
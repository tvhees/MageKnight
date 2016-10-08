using Commands;
using Other.Data;
using Other.Factory;
using Other.Utility;
using Prototype.NetworkLobby;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using View;

public class GameController : NetworkBehaviour
{
    public static GameController singleton;

    #region Controller

    public GamePlayers players { get; private set; }
    public GameDice dice { get; private set; }

    #endregion Controller

    #region Model

    public Board board;
    public Cards cards;
    public StateController stateController;
    public CommandStack commandStack;

    #endregion Model

    #region View

    public BoardView boardView;
    public SharedView sharedView;
    public ToolTip toolTip;
    public GameObject zoomDisplayPanel;

    #endregion View

    #region Settings

    public int randomSeed;

    public Scenario scenario
    {
        get { return players.Total <= 1 ? ScenarioDatabase.GetScriptableObject("Solo Conquest") : ScenarioDatabase.GetScriptableObject("Full Conquest"); }
    }

    #endregion Settings

    public GameConstants.TerrainCosts movementCosts = new GameConstants.TerrainCosts(true);
    public CardFactory cardFactory;
    public DebugPanel debugPanel;

    #region Initialisation

    private void Awake()
    {
        singleton = this;
        players = GetComponent<GamePlayers>();
        dice = GetComponent<GameDice>();
        AddEventListeners();
    }

    private void AddEventListeners()
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
    private IEnumerator ServerWaitForConnections()
    {
        while (!players.AllConnected)
            yield return null;

        StartGame();
    }

    [Server]
    private int CalculateExpectedPlayers()
    {
        int i = 0;
        foreach (NetworkLobbyPlayer player in LobbyManager.s_Singleton.lobbySlots)
        {
            if (player != null)
                i++;
        }
        return i;
    }

    [Server]
    public void StartGame()
    {
        players.RandomiseOrder();
        stateController.ChangeToState(GameConstants.GameState.CharacterSelect);
    }

    #endregion Initialisation

    #region Game setup

    [Server]
    public void CreateGameFromRandomSeed()
    {
        // If we haven't specified a seed its value will be 0 and we should create a new one
        if (randomSeed == 0)
            randomSeed = System.Environment.TickCount;
        Random.InitState(randomSeed);

        board = new Board(scenario, players, boardView);
        cards = new Cards(scenario, players);
        var mana = new ManaPool(players);

        dice.Enable(mana, sharedView);
    }

    #endregion Game setup

    #region SelectionPhases

    private void UiSelectCharacter(string characterName)
    {
        players.local.CmdSetCharacter(characterName);
    }

    [Server]
    public void OnCharacterSelected(string name)
    {
        var endRound = players.OnLastForRound;
        sharedView.RpcDisableButton(name);
        players.AssignCharacter();
        if (endRound)
            stateController.ChangeToState(GameConstants.GameState.BoardSetup);
    }

    private void UiSelectTactic(string tacticName)
    {
        players.local.CmdSetTactic(tacticName);
    }

    [Server]
    public void OnTacticSelected(string name)
    {
        var endRound = players.OnLastForRound;
        sharedView.RpcDisableButton(name);
        var tactic = CardDatabase.GetScriptableObject(name);
        players.AssignTactic(cards, tactic);
        if (endRound)
            stateController.ChangeToState(GameConstants.GameState.TurnSetup);
    }

    #endregion SelectionPhases

    #region Mana

    [Client]
    public void UiDieToggled(ManaId manaId)
    {
        players.local.CmdDieToggled(manaId);
    }

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
            ReturnManaSource(dice.usedDice[i]);
            dice.RollDie(dice.usedDice[i].index);
        }

        dice.usedDice.Clear();
    }

    #endregion Mana

    public void EndTurn()
    {
        stateController.ChangeToState(stateController.endOfTurn);
        players.EndTurn();
        ReturnAllDice();
        stateController.ChangeToState(stateController.turnSetup);
    }

    #region Command and effect management

    [Server]
    public void EnableUndo(bool enable)
    {
        players.current.view.RpcEnableUndo(enable);
    }

    public void UiPlayEffect(CardId cardId)
    {
        players.local.CmdPlayCard(cardId);
    }

    #endregion Command and effect management
}
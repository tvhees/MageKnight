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
    public GamePlayers players;
    #endregion

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
    public Scenario scenario {
        get {
            if (players.Total <= 1)
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
        players.RandomiseOrder();
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
        cards = new Cards(scenario, players);
        mana = new ManaPool(players);
        sharedView.RpcEnableDice(mana.dice.Length);
    }
    #endregion

    #region SelectionPhases
    void UiSelectCharacter(string name)
    {
        players.local.CmdSetCharacter(name);
    }

    [Server]
    public void OnCharacterSelected(string name)
    {
        sharedView.RpcDisableButton(name);
        players.AssignCharacter();
        if (players.OnLastForRound)
            stateController.ChangeToState(GameConstants.GameState.BoardSetup);
    }

    void UiSelectTactic(string name)
    {
        players.local.CmdSetTactic(name);
    }

    [Server]
    public void OnTacticSelected(string name)
    {
        sharedView.RpcDisableButton(name);
        var tactic = CardDatabase.GetScriptableObject(name);
        players.AssignTactic(cards, tactic);

        if (players.OnLastForRound)
            stateController.ChangeToState(GameConstants.GameState.TurnSetup);
    }
    #endregion

    #region Player management
    void UiEndTurn()
    {
        players.local.CmdEndTurn();
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
        players.local.CmdDieToggled(manaId.selected);

        if (manaId.selected)
            players.local.CmdAddMana(manaId.colour);
        else
            players.local.CmdRemoveMana(manaId.colour);
    }
    #endregion

    #region Command and effect management
    [Server]
    public void EnableUndo(bool enable)
    {
        players.current.view.RpcEnableUndo(enable);
    }

    public void UiPlayEffect(CardId cardId)
    {
        players.local.CmdPlayEffect(cardId);
    }
    #endregion
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager> {

    // ****************
    // INITIALISATION
    // ****************
    public int numberOfPlayers = 1;

    void Awake()
    {
        SetState(State.start);
        SetTimeOfDay(true);
        GetGameRules();
        NewBoard();
    }

    // ****************
    // GAME STATE
    // ****************
    public enum State
    {
        start,
        play,
        end
    }
    public State m_state;
    private State m_oldState;

    /// <summary>
    /// Return the current game state
    /// </summary>
    /// <returns></returns>
    public State GetState()
    {
        return m_state;
    }

    /// <summary>
    /// Set the game state. Stores the old game state for use with RevertState() by default
    /// </summary>
    /// <param name="newState"></param>
    public void SetState(State newState, bool save = true)
    {
        if (save)
        {
            m_oldState = m_state;
        }

        m_state = newState;
    }

    /// <summary>
    /// Set gamestate to last saved state
    /// </summary>
    public void RevertState()
    {
        SetState(m_oldState);
    }

    private static bool dayTime;

    public static bool IsDayTime()
    {
        return dayTime;
    }

    public static void SetTimeOfDay(bool isDayTime)
    {
        dayTime = isDayTime;
    }

    // ****************
    // PLAYERS
    // ****************
    public GameObject m_playerPrefab;

    private List<PlayerManager> m_players = new List<PlayerManager>();
    private int currentPlayer = 0;

    /// <summary>
    /// Add n players to the game
    /// </summary>
    /// <param name="n"></param>
    void CreatePlayers(int n)
    {
        // Remove any existing players
        while(m_players.Count > 0)
            RemovePlayer(0);

        // Add as many players as required
        for (int i = 0; i < n; i++)
        {
            PlayerManager newPlayer = Instantiate(m_playerPrefab).GetComponent<PlayerManager>();
            newPlayer.Init(i);
            AddPlayer(newPlayer);
        }
    }

    /// <summary>
    /// Store reference to a new player's script
    /// </summary>
    /// <param name="playerRef"></param>
    void AddPlayer(PlayerManager playerRef)
    {
        m_players.Add(playerRef);
    }

    /// <summary>
    /// Remove the i-th player from the game
    /// </summary>
    /// <param name="i"></param>
    void RemovePlayer(int i)
    {
        Destroy(m_players[i].gameObject);
        m_players.RemoveAt(i);
    }

    public PlayerManager GetCurrentPlayer()
    {
        return m_players[currentPlayer];
    }

    // ****************
    // GAME RULES
    // ****************
    public static CardManager m_cardManager;
    public static Movement m_movement;
    public static Combat m_combat;

    /// <summary>
    /// Get components controlling each phase of the game
    /// </summary>
    void GetGameRules()
    {
        m_cardManager = GetComponent<CardManager>();
        m_movement = GetComponent<Movement>();
        m_combat = GetComponent<Combat>();
    }

    // ****************
    // GAME BOARD
    // ****************

    public GameObject m_boardPrefab;
    private BoardManager m_boardManager;

    /// <summary>
    /// Create a new board, removing any existing board tile and components
    /// Also adds the required number of playerss
    /// </summary>
    public void NewBoard()
    {
        if (m_boardManager != null)
        {
            m_boardManager.RemoveBoard();
            m_cardManager.ShuffleDecks();
        }
        else
            m_cardManager.Init();

        m_boardManager = Instantiate(m_boardPrefab).GetComponent<BoardManager>();

        CreatePlayers(numberOfPlayers);
    }
}

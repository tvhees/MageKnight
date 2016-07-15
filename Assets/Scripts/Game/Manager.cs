using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BoardGame
{
    namespace Game
    {
        public class Manager : Singleton<Manager>
        {

            // ****************
            // INITIALISATION
            // ****************
            public Camera m_sharedCamera;
            public int numberOfPlayers = 1;

            void Awake()
            {
                SetState(State.start);
                if (Enemy.Manager.Instance != null)
                {
                    Enemy.Manager.Instance.Init();
                }

                if (Board.Manager.Instance != null)
                {
                    Board.Manager.Instance.CreateBoard();
                }

                if (Cards.Manager.Instance != null)
                {
                    Cards.Manager.Instance.Init();
                }

                CreatePlayers(numberOfPlayers);
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
            private State m_state;
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

            // ****************
            // PLAYERS
            // ****************
            public GameObject m_playerPrefab;

            private List<Players.Player> m_players = new List<Players.Player>();
            private int currentPlayer = 0;

            /// <summary>
            /// Add n players to the game
            /// </summary>
            /// <param name="n"></param>
            void CreatePlayers(int n)
            {
                // Remove any existing players
                while (m_players.Count > 0)
                    RemovePlayer(0);

                // Add as many players as required
                for (int i = 0; i < n; i++)
                {
                    Players.Player newPlayer = Instantiate(m_playerPrefab).GetComponent<Players.Player>();
                    newPlayer.Init(i);
                    AddPlayer(newPlayer);
                }                
            }

            /// <summary>
            /// Store reference to a new player's script
            /// </summary>
            /// <param name="playerRef"></param>
            void AddPlayer(Players.Player playerRef)
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

            public Players.Player GetCurrentPlayer()
            {
                return m_players[currentPlayer];
            }

            public List<Players.Player> GetAllPlayers()
            {
                return m_players;
            }

            public void NextPlayer()
            {
                currentPlayer = (int)Mathf.Repeat(currentPlayer + 1, numberOfPlayers);
            }
        }
    }
}
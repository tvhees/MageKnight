using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
    namespace Game
    {
        public class Game
        {
            public static Rules rules;
            public Camera sharedCamera;
            public int numberOfPlayers = 1;

            public static float unitOfDistance = 2.0f;

            void Awake()
            {
                SetState(State.start);

                if (GetComponent<Rules>() == null)
                    gameObject.AddComponent<Rules>();

                if (Enemy.Manager.Instance != null)
                {
                    Enemy.Manager.Instance.Init();
                }

                if (Board.Creator.Instance != null)
                {
                    Board.Creator.Instance.CreateBoard();
                }

                if (Card.Manager.Instance != null)
                {
                    Card.Manager.Instance.Init();
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
            private State state;
            private State oldState;

            /// <summary>
            /// Return the current game state
            /// </summary>
            /// <returns></returns>
            public State GetState()
            {
                return state;
            }

            /// <summary>
            /// Set the game state. Stores the old game state for use with RevertState() by default
            /// </summary>
            /// <param name="newState"></param>
            public void SetState(State newState, bool save = true)
            {
                if (save)
                {
                    oldState = state;
                }

                state = newState;
            }

            /// <summary>
            /// Set gamestate to last saved state
            /// </summary>
            public void RevertState()
            {
                SetState(oldState);
            }

            // ****************
            // PLAYERS
            // ****************
            public GameObject m_playerPrefab;

            private static List<Players.Player> players = new List<Players.Player>();
            private static int currentPlayer = 0;

            void CreatePlayers(int n)
            {
                while (players.Count > 0)
                    RemovePlayerFromGame(0);

                for (int i = 0; i < n; i++)
                {
                    Players.Player newPlayer = Instantiate(m_playerPrefab).GetComponent<Players.Player>();
                    newPlayer.Init(i);
                    AddPlayer(newPlayer);
                }                
            }

            void AddPlayer(Players.Player playerRef)
            {
                players.Add(playerRef);
            }

            void RemovePlayerFromGame(int i)
            {
                Destroy(players[i].gameObject);
                players.RemoveAt(i);
            }

            public static Players.Player GetCurrentPlayer()
            {
                return players[currentPlayer];
            }

            public List<Players.Player> GetAllPlayers()
            {
                return players;
            }

            public void NextPlayer()
            {
                currentPlayer = (int)Mathf.Repeat(currentPlayer + 1, numberOfPlayers);
            }
        }
    }
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
    public class GameImpl : MonoBehaviour, Game
    {
        public Camera sharedCamera;

        private Board.Board board;
        private Players players;
        private Model.Turn turn;
        public static float unitOfDistance = 2.0f;

        private Rulesets.Ruleset rules;

        // Eventually move to a Command pattern
        public Rulesets.Ruleset Rules
        {
            get { return rules; }
        }

        void Awake()
        {
            board = FindObjectOfType<Board.Board>();
            players = FindObjectOfType<Players>();
            turn = FindObjectOfType<Model.Turn>();
        }

        public void StartScenario(Board.Scenario scenario, int numberOfPlayers)
        {
            board.CreateBoard(scenario, numberOfPlayers);

            players.CreatePlayers(numberOfPlayers);

            turn.StartNewTurn();
        }

        public void EndScenario()
        {
            board.DestroyBoard();
        }

        public void SetCurrentRules(Rulesets.Ruleset rulesForThisPhase)
        {
            rules = rulesForThisPhase;
        }

        public enum State
        {
            start,
            play,
            end
        }
        private State state;
        private State previousState;

        public State GetState()
        {
            return state;
        }

        public void SetState(State newState, bool save = true)
        {
            if (save)
            {
                previousState = state;
            }

            state = newState;
        }

        public void RevertState()
        {
            SetState(previousState);
        }
    }
}
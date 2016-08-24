using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
    public class GameImpl : MonoBehaviour
    {
        public Camera sharedCamera;

        public static float unitOfDistance = 2.0f;

        public Rulesets.BaseRuleset rules;

        public void StartScenario(Board.Scenario scenario)
        {
            Main.board.CreateBoard(scenario);

            Main.cards.CreateSharedDecks();

            Main.cardShop.FillUnitOffer();

            Main.players.CreatePlayers();

            Main.turn.StartNewTurn();
        }

        public void EndScenario()
        {
            Main.board.DestroyBoard();
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
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
namespace Boardgame.Game
{
    public class Turn : MonoBehaviour
    {
        TurnState startState;
        TurnState movementState;
        TurnState influenceState;
        TurnState combatState;
        TurnState endState;

        TurnState state;

        public UnityEvent stateChanged;

        public Turn()
        {
            startState = new StartState(this);
            movementState = new MovementState(this);
            influenceState = new InfluenceState(this);
            combatState = new CombatState(this);
            endState = new EndState(this);
        }

        public void SetState(TurnState state)
        {
            this.state = state;
            NotifyListeners();
        }

        public void NotifyListeners()
        {
            stateChanged.Invoke();
        }

        public Rulesets.Ruleset GetRuleSet()
        {
            return state.GetRuleset();
        }

        public void EndCurrentState()
        {
            state.CleanUpState();
            state.EndCurrentState();
        }

        public void EndTurn()
        {
            // End the current turn
        }

        public TurnState GetStartState()
        {
            return startState;
        }

        public TurnState GetMovementState()
        {
            return movementState;
        }
        public TurnState GetInfluenceState()
        {
            return influenceState;
        }
        public TurnState GetCombatState()
        {
            return combatState;
        }

        public TurnState GetEndState()
        {
            return endState;
        }

        public void RegisterListener(UnityAction listenerMethod)
        {
            stateChanged.AddListener(listenerMethod);
        }

        public void RemoveListener(UnityAction listenerMethod)
        {
            stateChanged.RemoveListener(listenerMethod);
        }
    }
}
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
namespace Boardgame.Model
{
    [System.Serializable]
    public class StateChanged : UnityEvent<Rulesets.Ruleset> { }

    public class Turn : MonoBehaviour
    {

        public Rulesets.BaseRuleset baseRuleset;
        public TurnState state;
        public StateChanged stateChanged;

        [SerializeField] private TurnState startState;
        [SerializeField] private TurnState movementState;
        [SerializeField] private TurnState influenceState;
        [SerializeField] private TurnState combatState;
        [SerializeField] private TurnState endState;

        public void StartNewTurn()
        {
            SetState(startState);
        }

        public void SetState(TurnState state)
        {
            this.state = state;
            state.StartState();
            stateChanged.Invoke(GetRuleSet());
        }

        Rulesets.Ruleset GetRuleSet()
        {
            return state.GetRuleset(baseRuleset);
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
    }
}
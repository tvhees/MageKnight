﻿using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
namespace Boardgame.Model
{
    public class TurnEvent : UnityEvent { }

    public class Turn : MonoBehaviour
    {
        public Rulesets.BaseRuleset baseRuleset;
        public TurnState currentState;
        [HideInInspector] public TurnState backupState;

        [SerializeField] private TurnState startState;
        [SerializeField] private TurnState movementState;
        [SerializeField] private TurnState influenceState;
        [SerializeField] private TurnState combatState;
        [SerializeField] private TurnState endState;

        void Awake()
        {
            Main.turn = this;
            Main.turnStart.AddListener(TurnStart);
        }

        public void TurnStart(PlayerImpl player)
        {
            SetState(startState);
        }

        public void SetState(TurnState state)
        {
            currentState = state;
            state.BeginState();
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
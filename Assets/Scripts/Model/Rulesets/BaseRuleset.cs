using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Boardgame.Rulesets
{
    public class BaseRuleset : MonoBehaviour, Ruleset
    {
        private Commands.Stack commandStack;
        private Players players;

        void Awake()
        {
            commandStack = FindObjectOfType<Commands.Stack>();
            players = FindObjectOfType<Players>();
        }

        public Commands.Stack CommandStack
        {
            get { return commandStack; }
        }

        public Players Players
        {
            get { return players; }
        }

        public void AddMovement(EffectData input)
        {
            Debug.Log("Adding " + input.intValue + " movement points.");
        }

        public void AddInfluence(EffectData input)
        {
            Debug.Log("Adding " + input.intValue + " influence points.");
        }

        public void AddAttack(EffectData input)
        {
            Debug.Log("Adding " + input.intValue + " attack points.");
        }

        public void AddBlock(EffectData input)
        {
            Debug.Log("Adding " + input.intValue + " block points.");
        }

        public void AddHealing(EffectData input)
        {
            Debug.Log("Adding " + input.intValue + " healing points.");
        }

        public void AddOrRemoveEnemyFromCombatSelection(EffectData input)
        {
            Debug.Log("Adding or removing " + input.stringValue);
        }

        public void StartCombat(EffectData input)
        {
            Debug.Log("Starting Combat");
        }

        public void Provoke(EffectData input)
        {
            Debug.Log("Provoking Enemy");
        }

        public void HexClicked(EffectData input)
        {
            Debug.Log("Hex clicked: no generic action");
        }
    }
}
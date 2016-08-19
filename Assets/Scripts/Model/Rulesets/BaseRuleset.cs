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
        private Model.Turn turn;

        void Awake()
        {
            commandStack = FindObjectOfType<Commands.Stack>();
            players = FindObjectOfType<Players>();
            turn = FindObjectOfType<Model.Turn>();
        }

        public Commands.Stack CommandStack
        {
            get { return commandStack; }
        }

        public Players Players
        {
            get { return players; }
        }

        public Model.Turn Turn
        {
            get { return turn; }
        }

        public void AddMovement(EffectData input)
        {
            int movement = input.intValue;
            CommandStack.AddCommand(new AddMovementToPlayer(Players.currentPlayer, movement));
        }

        public void AddInfluence(EffectData input)
        {
            int influence = input.intValue;
            CommandStack.AddCommand(new AddInfluenceToPlayer(Players.currentPlayer, influence));
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
            int healing = input.intValue;
            CommandStack.AddCommand(new AddHealingToPlayer(Players.currentPlayer, healing));
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
            GameObject enemy = input.gameObjectValue;
            float squareDistance = (enemy.transform.position - Players.currentPlayer.position).sqrMagnitude;

            if (Mathf.Sqrt(squareDistance) < GameImpl.unitOfDistance)
            {
                Main.commandStack.AddCommand(new AddEnemyToCombat(enemy));
            }
            else
                Debug.Log(string.Format("{0} is too far away to be provoked", enemy.name));
        }

        public void Interact(EffectData input)
        {
            GameObject hex = input.gameObjectValue;
            Board.InteractibleFeature interactible = hex.GetComponentInChildren<Board.InteractibleFeature>();
            if (interactible != null)
            {
                interactible.ExecuteInteraction();
            }
            else
                MoveToTile(input);
        }

        public void MoveToTile(EffectData input)
        {
            GameObject tile = input.gameObjectValue;
            CommandStack.AddCommand(new MoveToTile(tile));
        }

        public void UseShop(EffectData input)
        {
            GameObject shop = input.gameObjectValue;

            float squareDistance = (shop.transform.position - Players.currentPlayer.position).sqrMagnitude;

            if (Mathf.Sqrt(squareDistance) < 0.5f * GameImpl.unitOfDistance)
                Main.commandStack.AddCommand(new InfluenceLocals(shop));
            else if (Mathf.Sqrt(squareDistance) < GameImpl.unitOfDistance)
                Main.commandStack.AddCommand(new MoveToTile(shop.transform.parent.gameObject));
            else
                Debug.Log(string.Format("{0} is too far away to move to", shop.name));
        }
    }
}
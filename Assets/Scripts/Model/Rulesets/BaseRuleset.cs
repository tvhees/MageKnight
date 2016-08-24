using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Boardgame.Rulesets
{
    public class BaseRuleset : MonoBehaviour
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

        public void AddInfluence(int input)
        {
            CommandStack.AddCommand(new AddInfluenceToPlayer(Players.currentPlayer, input));
        }

        public void AddAttack(EffectData input)
        {
            Debug.Log("Adding " + input.intValue + " attack points.");
        }

        public void AddBlock(EffectData input)
        {
            Debug.Log("Adding " + input.intValue + " block points.");
        }

        public void AddHealing(int input, int cost)
        {
            CommandStack.AddCommand(new AddHealingToPlayer(Players.currentPlayer, input, cost));
        }

        public void AddReputation(int input)
        {
            CommandStack.AddCommand(new AddReputationToPlayer(Players.currentPlayer, input));
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

        public void UseShop(Board.ShoppingLocation input)
        {
            float squareDistance = (input.transform.position - Players.currentPlayer.position).sqrMagnitude;

            if (Mathf.Sqrt(squareDistance) < 0.5f * GameImpl.unitOfDistance)
                Main.cardShop.OpenShop(input.type);
            else if (Mathf.Sqrt(squareDistance) < GameImpl.unitOfDistance)
                Main.commandStack.AddCommand(new MoveToTile(input.transform.parent.gameObject));
            else
                Debug.Log(string.Format("{0} is too far away to move to", input.name));
        }

        public void PlunderVillage()
        {
            AddReputation(-1);
            Main.players.currentPlayer.DrawCards(2);
        }
    }
}
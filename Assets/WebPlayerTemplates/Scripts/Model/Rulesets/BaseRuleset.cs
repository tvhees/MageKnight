using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Boardgame.Rulesets
{
    public class BaseRuleset : MonoBehaviour
    {
        public float unitOfDistance = 2.0f;

        private Commands.Stack commandStack;
        private Model.Turn turn;

        void Awake()
        {
            Main.rules = this;
            commandStack = FindObjectOfType<Commands.Stack>();
            turn = FindObjectOfType<Model.Turn>();
        }

        public Commands.Stack CommandStack
        {
            get { return commandStack; }
        }

        public Model.Turn Turn
        {
            get { return turn; }
        }

        public void AddMovement(EffectData input)
        {
            int movement = input.intValue;
            CommandStack.RunCommand(new AddMovementToPlayer(input.player, movement));
        }

        public void AddInfluence(int input)
        {
            CommandStack.RunCommand(new AddInfluenceToPlayer(input));
        }

        public void AddAttack(EffectData input)
        {
            Debug.Log("Adding " + input.intValue + " attack points.");
        }

        public void AddBlock(EffectData input)
        {
            Debug.Log("Adding " + input.intValue + " block points.");
        }

        public void AddHealing(EffectData input, int cost)
        {
            CommandStack.RunCommand(new AddHealingToPlayer(input.player, input.intValue, cost));
        }

        public void AddReputation(EffectData input)
        {
            CommandStack.RunCommand(new AddReputationToPlayer(input.player, input.intValue));
        }

        public void AddOrRemoveEnemyFromCombatSelection(EffectData input)
        {
            Debug.Log("Adding or removing enemy");
        }

        public void StartCombat(EffectData input)
        {
            Debug.Log("Starting Combat");
        }

        public void Provoke(EffectData input)
        {
            GameObject enemy = input.gameObjectValue;
            float squareDistance = (enemy.transform.position - input.player.position).sqrMagnitude;

            if (Mathf.Sqrt(squareDistance) < unitOfDistance)
            {
                Main.commandStack.RunCommand(new AddEnemyToCombat(enemy));
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
            CommandStack.RunCommand(new MoveToTile(tile));
        }

        public void UseShop(EffectData input, Board.ShoppingLocation shop)
        {
            float squareDistance = (shop.transform.position - input.player.position).sqrMagnitude;

            if (Mathf.Sqrt(squareDistance) < 0.5f * unitOfDistance)
                Main.cardShop.OpenShop(shop.type);
            else if (Mathf.Sqrt(squareDistance) < unitOfDistance)
                Main.commandStack.RunCommand(new MoveToTile(shop.transform.parent.gameObject));
            else
                Debug.Log(string.Format("{0} is too far away to move to", shop.name));
        }

        public void PlunderVillage(EffectData input)
        {
            AddReputation(new EffectData(input.player, -1));
            input.player.DrawCards(2);
        }
    }
}
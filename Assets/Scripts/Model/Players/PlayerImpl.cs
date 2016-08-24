using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
    [System.Serializable]
    public class VariableUpdate : UnityEvent<Player.Variables, Player.Statistics> { }

    public class PlayerImpl: MonoBehaviour
	{
        public GameObject tile;
        public Player.Variables variables { get; private set; }
        public Player.Statistics statistics { get; private set; }
        public ObjectMover objectMover { get; private set; }
        public Player.Belongings belongings { get; private set; }

        public VariableUpdate variableUpdate = new VariableUpdate();

        private Player.VariablesDisplay varsDisplay;
        private GameObject playerObject;

        public Vector3 position { get { return playerObject.transform.position; } }
        Vector3 center { get { return position + new Vector3(0f, transform.localScale.y * 0.5f, 0f); } }

        void Awake()
        {
            statistics = new Player.Statistics();
            varsDisplay = FindObjectOfType<Player.VariablesDisplay>();
            varsDisplay.SubscribeToVariables(variableUpdate);
        }

        public void SetObject(GameObject playerObject)
        {
            this.playerObject = playerObject;
        }

        public void SetBelongings(Player.Belongings belongings)
        {
            this.belongings = belongings;
        }

        public void AddObjectMover()
        {
            objectMover = playerObject.GetComponent<ObjectMover>();
        }

        public void NewVariables()
        {
            variables = new Player.Variables();
            variableUpdate.Invoke(variables, statistics);
        }

        public bool AddInfluence(int value)
        {
            if (variables.influence + value >= 0)
            {
                variables.influence += value;
                variableUpdate.Invoke(variables, statistics);
                return true;
            }
            else
                return false;
        }

        public void AddMovement(int value)
        {
            variables.movement += value;
            variableUpdate.Invoke(variables, statistics);
        }

        public void AddHealing(int value)
        {
            variables.healing += value;
            variableUpdate.Invoke(variables, statistics);
        }

        public int AddReputation(int value)
        {
            Debug.Log("Adding rep: " + value);

            int oldReputation = statistics.reputation;
            statistics.reputation += value;

            variableUpdate.Invoke(variables, statistics);

            int changeInReputation = statistics.reputation - oldReputation;
            return changeInReputation;
        }

        public void DrawCards(int cardsToDraw = 1)
        {
            for (int i = 0; i < cardsToDraw; i++)
            {
                GameObject card = belongings.deckPanel.transform.LastChild();
                var cardController = card.GetComponent<Cards.MovementAndDisplay>();
                cardController.MoveToNewParent(belongings.handPanel.transform);
                cardController.ShowFront();

                Main.commandStack.ClearCommandList();
            }
        }

        public bool CanMoveToTile(GameObject tile)
        {
            if (objectMover.moving)
                return false;

            if (tile == this.tile)
                return false;

            if (Vector3.Magnitude(tile.transform.position - position) > variables.movementRange)
                return false;

            Board.Terrain terrain = tile.GetComponent<Board.Terrain>();

            if (!terrain.isTraversable)
                return false;

            if (terrain.movementCost > variables.movement)
                return false;

            return true;
        }

        public void MoveToTile(GameObject tile)
        {
            objectMover.AddDestination(tile.transform.position);
            this.tile = tile;
        }
    }
}
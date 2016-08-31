using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
    [System.Serializable]
    public class VariableUpdateEvent : UnityEvent<Player.Variables, Player.Statistics> { }

    public class PlayerImpl : NetworkBehaviour
	{
        [SyncVar] public int playerId;

        static List<PlayerImpl> listOfPlayers = new List<PlayerImpl>();

        public GameObject playerViewPrefab;
        public GameObject tile;
        public Player.Variables variables { get; private set; }
        public Player.Statistics statistics { get; private set; }
        public ObjectMover playerObjectMover { get; private set; }
        public Player.Belongings belongings { get; private set; }

        public VariableUpdateEvent variableUpdate = new VariableUpdateEvent();

        private GameObject playerObject;
        public GameObject playerView;

        public Vector3 position { get { return playerObject.transform.position; } }
        Vector3 center { get { return position + new Vector3(0f, transform.localScale.y * 0.5f, 0f); } }

        #region Creation
        public override void OnStartLocalPlayer()
        {
            tile = GameObject.FindGameObjectWithTag("PortalTile");

            playerObject = transform.GetChild(0).gameObject;

            statistics = new Player.Statistics();

            Main.turnStart.AddListener(ResetVariables);

            transform.Translate(Random.onUnitSphere);
        }

        void CreatePlayerObject(GameObject playerPrefab)
        {
            playerObject = transform.InstantiateChild(playerPrefab);
            playerObjectMover = playerObject.GetComponent<ObjectMover>();
        }

        public void CreateBelongings(GameObject playerViewPrefab)
        {
            playerView = Instantiate(playerViewPrefab, 100f * Vector3.left, Quaternion.identity) as GameObject;
            belongings = playerView.GetComponentInChildren<Player.Belongings>();

            List<GameObject> playerDeck = Main.cards.CreatePlayerDeck();
            foreach (GameObject card in playerDeck)
                belongings.GainCardToDeck(card.GetComponent<Cards.MovementAndDisplay>());
        }
        #endregion

        #region PlayerVariables
        public void ResetVariables(PlayerImpl currentPlayer)
        {
            if (currentPlayer = this)
            {
                variables = new Player.Variables();
                variableUpdate.Invoke(variables, statistics);
            }
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
            int oldReputation = statistics.reputation;
            statistics.reputation += value;

            variableUpdate.Invoke(variables, statistics);

            int changeInReputation = statistics.reputation - oldReputation;
            return changeInReputation;
        }

        #endregion

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

        #region PlayerMovement
        public bool CanMoveToTile(GameObject tile)
        {
            if (playerObjectMover.moving)
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
            playerObjectMover.AddDestination(tile.transform.position);
            this.tile = tile;
        }
        #endregion

        #region NetworkDebugging
        void Update()
        {
            if (!isLocalPlayer)
                return;

            if (Input.GetKey(KeyCode.UpArrow))
                transform.Translate(Vector3.up);
        }
        #endregion
    }
}
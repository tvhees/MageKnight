using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
    namespace Board
    {
        public class Manager : Singleton<Manager>
        {

            // ****************
            // CONSTRUCTION
            // ****************
            public GameObject boardPrefab; // Main prefab of board goes here if one is required
            private GameObject boardHolder;

            public void CreateBoard()
            {
                if (boardHolder != null)
                    RemoveBoard();

                boardHolder = new GameObject("BoardHolder"); // Folder object for all board components - could be a complete prefab if desired
                if (boardPrefab != null)
                {
                    AddBoardComponent(Instantiate(boardPrefab));
                }

                HexGrid.Factory.Instance.LoadXML();
                HexGrid.Factory.Instance.CreateRandomStacks();

                // Place starting tile
                AddBoardComponent(HexGrid.Factory.Instance.GetHexGroup(0, new HexGrid.HexCoordinates(0, 0, 0)));

                // Place first two countryside tiles
                AddBoardComponent(HexGrid.Factory.Instance.GetHexGroup(false, new HexGrid.HexCoordinates(3, 2, 0)));
                AddBoardComponent(HexGrid.Factory.Instance.GetHexGroup(false, new HexGrid.HexCoordinates(3, 0, 1)));
            }

            // Use this to add further components as children of the BoardHolder object
            void AddBoardComponent(GameObject component)
            {
                component.transform.SetParent(boardHolder.transform);
            }

            // ****************
            // DESTRUCTION
            // ****************
            public void RemoveBoard()
            {
                Destroy(boardHolder);
            }
        }
    }
}
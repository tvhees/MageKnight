using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
    namespace Board
    {
        public class Creator : MonoBehaviour
        {
            public string scenarioName = "FullConquest";

            private GameObject boardHolder;
            private HexTile.GroupFactory hexGroupFactory;

            public void CreateBoard()
            {
                DestroyAnyExistingBoard();
                boardHolder = new GameObject("BoardHolder");

                hexGroupFactory = GetComponent<HexTile.GroupFactory>();
                hexGroupFactory.LoadData(scenarioName);
                hexGroupFactory.CreateRandomStacks();

                AddStartingTile();
                AddVisibleTiles();
            }

            void AddStartingTile()
            {
                AddBoardComponent(hexGroupFactory.GetGroupOfHexTiles(0, new HexTile.HexCoordinates(0, 0, 0)));
            }

            void AddVisibleTiles()
            {
                AddBoardComponent(hexGroupFactory.GetHexGroup(false, new HexTile.HexCoordinates(3, 2, 0)));
                AddBoardComponent(hexGroupFactory.GetHexGroup(false, new HexTile.HexCoordinates(3, 0, 1)));
            }

            // Use this to add further components as children of the BoardHolder object
            void AddBoardComponent(GameObject component)
            {
                component.transform.SetParent(boardHolder.transform);
            }

            public void OnDisable()
            {
                if (boardHolder != null)
                    Destroy(boardHolder);
            }
        }
    }
}
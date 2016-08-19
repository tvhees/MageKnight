using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Board
{
    public class HexTileDatabase : ScriptableObjectDatabase<HexTile>
    {
        void Awake()
        {
            path = "HexTiles";
        }
    }
}
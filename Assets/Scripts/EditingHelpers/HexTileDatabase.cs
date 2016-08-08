using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
    namespace Board
    {
        public class HexTileDatabase : ScriptableObjectDatabase<HexTile>
        {
            void Awake()
            {
                path = "HexTiles";
            }
        }
    }
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Board
{
    public class MapShapeDatabase: ScriptableObjectDatabase<MapShape>
	{
        void Awake()
        {
            path = "MapShapes";
        }
	}
}
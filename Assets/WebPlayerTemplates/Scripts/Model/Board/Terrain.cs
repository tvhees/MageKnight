using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Board
{
    public class Terrain: MonoBehaviour 
	{
        public Rulesets.TerrainType type;

        public bool isTraversable
        {
            get { return Rulesets.MovementCosts.IsTraversable(type); }
        }

        public int movementCost
        {
            get { return Rulesets.MovementCosts.GetCost(type); }
        }
	}
}
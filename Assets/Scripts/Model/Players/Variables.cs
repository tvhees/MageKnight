using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Player
{
    public class Variables 
	{
        public Variables()
        {
            movement = 0;
            influence = 0;
            movementRange = 2f;
        }

        public int movement;
        public int influence;
        public float movementRange;
	}
}
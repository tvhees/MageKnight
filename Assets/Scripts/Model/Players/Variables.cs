using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Player
{
    public class Variables 
	{
        public int movement;
        public int influence;
        public int healing;
        public float movementRange;

        public Variables()
        {
            movement = 0;
            influence = 0;
            healing = 0;
            movementRange = 2f;
        }
	}
}
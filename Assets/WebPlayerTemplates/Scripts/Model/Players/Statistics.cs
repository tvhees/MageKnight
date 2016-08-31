using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Player
{
    public class Statistics 
	{
        public int level;

        public int reputation
        {
            get { return reputationLevel; }
            set { reputationLevel = Mathf.Clamp(value, -8, 8); }
        }

        private int reputationLevel;

        public Statistics()
        {
            level = 1;
            reputationLevel = 0;
        }
    }
}
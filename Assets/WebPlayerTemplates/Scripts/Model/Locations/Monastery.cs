using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Board
{
    public class Monastery: MonoBehaviour 
	{
        public static int standingMonasteries = 0;

        public void MonasteryPlaced()
        {
            standingMonasteries++;

            Main.cardShop.AddMonasteryAction();

            Debug.Log(standingMonasteries);
        }

        void MonasteryDestroyed()
        {
            standingMonasteries--;
        }
	}
}
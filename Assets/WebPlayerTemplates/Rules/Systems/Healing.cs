using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
	namespace Rules
    {
		public class Healing : Singleton<Healing> 
		{
            public int totalPaid { get; private set; }

            public void AddHealing(int value)
            {
                totalPaid += value;
            }
        }
	}
}
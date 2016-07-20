using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BoardGame
{
	namespace Effect
    {
		public class Type : MonoBehaviour 
		{
            private string type; // basic,advanced, spell, artifact, common, elite 

            public string effectType
            {
                get { return type; }
                set { type = value; }
            }
		}
	}
}
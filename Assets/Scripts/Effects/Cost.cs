using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BoardGame
{
	namespace Effect
    {
		public class Cost : MonoBehaviour 
		{
            private string[] colours;

            public string[] costColours
            {
                get { return colours; }
                set { colours = value; }
            }
		}
	}
}
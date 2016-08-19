using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
    public class DisplayableFeature: MonoBehaviour 
	{
        public int layoutHeight;
        public string displayName = "Feature";

        [TextArea(1, 10)]
        public string description = "A location or enemy";
	}
}
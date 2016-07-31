using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Effect
{
	public struct EffectData 
	{
        public EffectData(int intValue = 0, string stringValue = "")
        {
            this.intValue = intValue;
            this.stringValue = stringValue;
        }

        public int intValue;
        public string stringValue;
	}
}
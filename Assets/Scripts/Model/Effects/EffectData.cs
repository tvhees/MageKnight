using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
	public struct EffectData 
	{
        public EffectData(int intValue = 0, string stringValue = "", Vector3 positionValue = default(Vector3), GameObject gameObjectValue = default(GameObject))
        {
            this.intValue = intValue;
            this.stringValue = stringValue;
            this.positionValue = positionValue;
            this.gameObjectValue = gameObjectValue;
        }

        public int intValue;
        public string stringValue;
        public Vector3 positionValue;
        public GameObject gameObjectValue;
	}
}
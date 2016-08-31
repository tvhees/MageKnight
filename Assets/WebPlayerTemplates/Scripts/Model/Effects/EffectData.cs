using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
	public struct EffectData 
	{
        public EffectData(PlayerImpl player = null, int intValue = 0, GameObject gameObjectValue = default(GameObject))
        {
            this.player = player;
            this.intValue = intValue;
            this.gameObjectValue = gameObjectValue;
        }

        public PlayerImpl player;
        public int intValue;
        public GameObject gameObjectValue;
	}
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
	namespace Enemy
    {
		public class Rampaging : Board.InteractibleFeature 
		{
            public override void ExecuteInteraction()
            {
                //Main.rules.Provoke(new EffectData(gameObjectValue: gameObject));
            }
        }
	}
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Board
{
    public class Shop : InteractibleFeature 
	{
        public override void ExecuteInteraction()
        {
            Main.rules.UseShop(new EffectData(gameObjectValue: gameObject));
        }
    }
}
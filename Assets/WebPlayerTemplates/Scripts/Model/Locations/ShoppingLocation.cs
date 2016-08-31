using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Board
{
    public class ShoppingLocation : InteractibleFeature 
	{
        public Cards.Shop.Type type;

        public override void ExecuteInteraction()
        {
            //Main.rules.UseShop(this);
        }
    }
}
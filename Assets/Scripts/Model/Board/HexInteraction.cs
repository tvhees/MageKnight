using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Board
{
    public class HexInteraction: MonoBehaviour 
	{
        public void Clicked()
        {
            Main.Instance.game.Rules.HexClicked(new EffectData(gameObjectValue: gameObject));
        }
	}
}
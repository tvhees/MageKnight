using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Board
{
    public class TilePlacedEvent : UnityEvent { }

    public class TileEventManager : MonoBehaviour 
	{
        public TilePlacedEvent tilePlacedEvent = new TilePlacedEvent();
	}
}
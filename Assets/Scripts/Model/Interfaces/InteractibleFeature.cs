using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Board
{
    public abstract class InteractibleFeature : MonoBehaviour 
	{
        public abstract void ExecuteInteraction();
	}
}
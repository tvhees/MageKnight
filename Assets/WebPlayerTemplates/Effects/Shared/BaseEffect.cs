using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
	namespace Effect
    {
		public interface BaseEffect 
		{
            void Execute();

            EffectData GetEffectData();
		}
	}
}
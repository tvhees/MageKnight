using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace BoardGame
{
	namespace GUI
    {
		public class PlayerArea : MonoBehaviour 
		{
            public Text m_strength;

            public void SetStrength(int value)
            {
                m_strength.text = value.ToString();
            }

		}
	}
}
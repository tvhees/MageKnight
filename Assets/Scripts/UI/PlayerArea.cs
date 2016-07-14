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
            public Text m_strengthPhys;
            public Text m_strengthCold;
            public Text m_strengthFire;
            public Text m_strengthColdFire;

            public void SetStrength(int value, string type)
            {
                Text textType = m_strengthPhys;
                switch (type)
                {
                    
                    case "physical":
                        textType = m_strengthPhys;
                        break;
                    case "cold":
                        textType = m_strengthCold;
                        break;
                    case "fire":
                        textType = m_strengthFire;
                        break;
                    case "coldfire":
                        textType = m_strengthColdFire;
                        break;
                }
                textType.enabled = true;
                textType.text = value.ToString();
            }

            public void ResetStrength()
            {
                m_strengthPhys.enabled = false;
                m_strengthCold.enabled = false;
                m_strengthFire.enabled = false;
                m_strengthColdFire.enabled = false;
            }

		}
	}
}
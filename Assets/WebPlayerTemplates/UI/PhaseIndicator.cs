using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
    public class PhaseIndicator : MonoBehaviour
    {
        public Color activeColour;
        public Color inactiveColour;


        private Text[] textArray;

        int index;

        void Awake()
        {
            textArray = GetComponentsInChildren<Text>();
        }

        public void StartCombat()
        {
            textArray[0].color = activeColour;

            index = 0;
        }

        // Turn off current phase text, activate next one.
        // Returns whether or not combat has ended.
        public bool NextPhase()
        {
            textArray[index].color = inactiveColour;
            index++;
            if (index >= textArray.Length)
            {
                return false;
            }
            else
            {
                textArray[index].color = activeColour;
                return true;
            }
        }

    }
}
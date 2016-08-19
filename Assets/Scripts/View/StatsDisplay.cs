using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Player
{
    public class StatsDisplay: MonoBehaviour 
	{
        public Text movement;
        public Text influence;
        public Text healing;

        public void SubscribeToVariables(UnityEvent<Variables> variablesUpdate)
        {
            variablesUpdate.AddListener(UpdateStats);
        }

        void UpdateStats(Variables variables)
        {
            movement.text = "Movement: " + variables.movement.ToString();
            influence.text = "Influence: " + variables.influence.ToString();
            healing.text = "Healing: " + variables.healing.ToString();
        }
	}
}
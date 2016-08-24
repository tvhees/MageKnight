using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Player
{
    public class VariablesDisplay: MonoBehaviour 
	{
        public Text movement;
        public Text influence;
        public Text healing;
        public Text level;
        public Text reputation;

        public void SubscribeToVariables(UnityEvent<Variables, Statistics> variablesUpdate)
        {
            variablesUpdate.AddListener(UpdateStats);
        }

        void UpdateStats(Variables variables, Statistics statistics)
        {
            movement.text = "Movement: " + variables.movement.ToString();
            influence.text = "Influence: " + variables.influence.ToString();
            healing.text = "Healing: " + variables.healing.ToString();
            level.text = "Level: " + statistics.level.ToString();
            reputation.text = "Reputation: " + statistics.reputation.ToString();
        }
	}
}
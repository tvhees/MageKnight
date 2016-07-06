using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BoardGame
{
	namespace Enemy
    {
		public class Object : MonoBehaviour 
		{
            private Enemy m_attributes;

            public void SetAttributes(Enemy input)
            {
                m_attributes = input;
            }

            public Attack GetAttack()
            {
                return m_attributes.attack;
            }

            public Defense GetDefense()
            {
                return m_attributes.defense;
            }

            public Reward GetReward()
            {
                return m_attributes.reward;
            }
        }
	}
}
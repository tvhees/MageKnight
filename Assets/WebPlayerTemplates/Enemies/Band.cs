using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
    namespace Enemy
    {
        public class Band
        {
            public Attack attack { get; private set; }
            public Defense m_defense { get; private set; }
            public Reward m_reward { get; private set; }

            private List<GUI.EnemyHolder> m_enemies;

            // Constructor for new empty instance
            public Band()
            {
                m_enemies = new List<GUI.EnemyHolder>();
            }

            public void AddOrRemoveEnemy(GUI.EnemyHolder enemyHolder)
            {
                if (m_enemies.Contains(enemyHolder))
                {
                    m_enemies.Remove(enemyHolder); // Remove the enemy from the combat instance
                }
                else
                {
                    m_enemies.Add(enemyHolder); // Add the enemy to the combat instance
                }

                SetProperties();
            }

            void SetProperties()
            {
                if(!IsEmpty())
                    attack = m_enemies[0].enemy.GetAttack(); // Only ever one attack at a time

                m_defense = new Defense(); // multiple defenders means we add their strength and properties
                foreach (GUI.EnemyHolder holder in m_enemies)
                {
                    m_defense += holder.enemy.GetDefense();
                }

                m_reward = Reward.NullReward(); // multiple defenders means we add rewards
                foreach (GUI.EnemyHolder holder in m_enemies)
                {
                    m_reward += holder.enemy.GetReward();
                }

            }

            public List<GUI.EnemyHolder> Enemies()
            {
                return m_enemies;
            }

            // Return true if this instance has no selected enemies
            public bool IsEmpty()
            {
                return m_enemies.Count == 0;
            }

            public void Disable()
            {
                for (int i = 0; i < m_enemies.Count; i++)
                {
                    m_enemies.GetLast().Disable();
                }
            }
        }
    }
}
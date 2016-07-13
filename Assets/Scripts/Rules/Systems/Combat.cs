using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace BoardGame
{
    namespace Rules
    {
        public class Combat : Singleton<Combat>
        {
            public GUI.CombatPanel m_combatPanel;

            public enum Phase
            {
                siege,
                ranged,
                block,
                attack,
                end
            }

            private Phase m_phase;

            public bool TestPhase(Phase testPhase)
            {
                if (m_phase == testPhase)
                    return true;
                else
                    return false;
            }

            // Lists to track all enemies in combat and those that have been selected
            private List<Enemy.Object> m_listOfEnemies;
            public EnemyBand m_currentInstance;
            private Players.Player m_currentPlayer;

            public IEnumerator StartCombat(Enemy.Object[] newEnemies)
            {
                m_currentPlayer = Game.Manager.Instance.GetCurrentPlayer();
                m_listOfEnemies = new List<Enemy.Object>(newEnemies); // Store the list of enemies being fought
                m_currentInstance = new EnemyBand(); // Create a new combat instance
                m_combatPanel.StartCombat(m_listOfEnemies); // Open the combat UI

                m_fame = 0;
                m_reputation = 0;

                StartCoroutine(PlayerAttack(Phase.siege)); // Start the first phase
                while (m_phase != Phase.end) // Don't pass control back to the main game until combat is finished
                {
                    yield return null;
                }
            }

            IEnumerator PlayerAttack(Phase thisPhase)
            {
                m_phase = thisPhase; // Start whichever phase has been called (siege, ranged, attack)

                while (m_phase == thisPhase) // Stay in this phase until the next has been started
                {
                    yield return null;
                }
            }

            public bool AddAttackOrBlock(int value) // Add to a player's total played block or attack
            {
                if (!m_currentInstance.IsEmpty())
                {
                    int pStrength = m_currentInstance.AddToPlayerTotal(value);
                    m_combatPanel.m_playerArea.SetStrength(pStrength); // update UI to reflect current strength
                    return true;
                }

                return false; // returns false if there's no selected enemies
            }

            IEnumerator PlayerBlock()
            {
                m_phase = Phase.block; // Start the block phase

                while (m_phase == Phase.block)
                {
                    yield return null;
                }
            }

            public void Resolve()
            {
                if (m_currentInstance.IsEmpty()) // If we've pressed this with no enemies selected, move on to the next phase
                {
                    NextPhase();
                    return;
                }
                else if (m_currentInstance.Successful()) // Otherwise evaluate whether the player has succeeded and proceed accordingly
                {
                    if (m_phase == Phase.block)
                        Debug.Log("attack blocked");
                    else
                        AddRewards(); // This enemy has been killed, at the end of combat we'll get something
                }
                else
                {
                    if (m_phase == Phase.block)
                        m_currentPlayer.TakeDamage(m_currentInstance.m_enemyTotal); // Unblocked attacks cause wounds
                    else
                        Debug.Log("enemy not defeated");
                }

                m_currentInstance.Disable(); // Disable all enemies in the current instance

                m_combatPanel.m_enemyArea.SelectNext(); // Select the next enemy in line
            }

            private int m_fame;
            private int m_reputation;

            void AddRewards()
            {
                m_fame += m_currentInstance.GetFame();
                m_reputation += m_currentInstance.GetFame();
                Debug.Log(m_fame + " fame and " + m_reputation + " reputation earned this combat");
            }

            void NextPhase()
            {
                // Move to the next phase of combat based on where we are now
                switch (m_phase)
                {
                    case Phase.siege:
                        StartCoroutine(PlayerAttack(Phase.ranged));
                        break;
                    case Phase.ranged:
                        StartCoroutine(PlayerBlock());
                        break;
                    case Phase.block:
                        m_combatPanel.m_enemyArea.ToggleGroup(true);
                        StartCoroutine(PlayerAttack(Phase.attack));
                        break;
                    case Phase.attack:
                        m_combatPanel.m_enemyArea.ToggleGroup(false);
                        EndCombat();
                        break;
                }

                // Update UI components
                m_combatPanel.NextPhase();
            }

            void EndCombat()
            {
                m_phase = Phase.end;
                m_combatPanel.gameObject.SetActive(false);
                m_currentPlayer.AddFame(m_fame);
                m_currentPlayer.AddReputation(m_reputation);
            }

        }

        public class EnemyBand
        {
            public Enemy.Attack m_attack { get; private set; }
            public Enemy.Defense m_defense { get; private set; }
            public Enemy.Reward m_reward { get; private set; }
            public int m_enemyTotal { get; private set; }

            private List<GUI.EnemyHolder> m_enemies;
            private int m_playerTotal;

            // Constructor for new empty instance
            public EnemyBand()
            {
                m_enemies = new List<GUI.EnemyHolder>();
                m_enemyTotal = 0;
                m_playerTotal = 0;
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
                m_attack = m_enemies[0].m_enemy.GetAttack(); // Only ever one attack at a time

                m_defense = new Enemy.Defense(); // multiple defenders means we add their strength and properties
                foreach (GUI.EnemyHolder holder in m_enemies)
                {
                    m_defense += holder.m_enemy.GetDefense();
                }

                m_reward = new Enemy.Reward(); // multiple defenders means we add rewards
                foreach (GUI.EnemyHolder holder in m_enemies)
                {
                    m_reward += holder.m_enemy.GetReward();
                }

            }

            // Return true if this instance has no selected enemies
            public bool IsEmpty()
            {
                return m_enemies.Count == 0;
            }

            // Add to player's attack or block total in this instance
            public int AddToPlayerTotal(int value)
            {
                m_playerTotal += value;

                return m_playerTotal;
            }

            public void Disable()
            {
                for(int i = 0; i < m_enemies.Count; i++)
                {
                    m_enemies.GetLast().Disable();
                }

                m_playerTotal = 0;
            }

            public bool Successful()
            {
                return m_playerTotal >= m_enemyTotal;
            }

            public int GetFame()
            {
                int fame = 0;
                foreach (GUI.EnemyHolder enemy in m_enemies)
                    fame += enemy.m_enemy.GetReward().fame;

                return fame;
            }

            public int GetReputation()
            {
                int rep = 0;
                foreach (GUI.EnemyHolder enemy in m_enemies)
                    rep += enemy.m_enemy.GetReward().reputation;

                return rep;
            }
        }
    }
}
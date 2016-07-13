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
            public CombatInstance m_currentInstance;

            public IEnumerator StartCombat(Enemy.Object[] newEnemies)
            {
                m_listOfEnemies = new List<Enemy.Object>(newEnemies); // Store the list of enemies being fought
                m_currentInstance = new CombatInstance(); // Create a new combat instance
                m_combatPanel.StartCombat(m_listOfEnemies); // Open the combat UI

                m_fame = 0;
                m_reputation = 0;

                StartCoroutine(PlayerAttack(Phase.siege)); // Start the first phase
                while (m_phase != Phase.end) // Don't pass control back to the main game until combat is finished
                {
                    yield return null;
                }
            }

            // Add together the strength of all enemies involved in a combat instance
            // During block phase this should only ever be one enemy.
            public int SumEnemies(List<GUI.EnemyHolder> input)
            {
                int total = 0;

                foreach (GUI.EnemyHolder obj in input)
                {
                    if (m_phase == Phase.block)
                        total += obj.m_enemy.GetAttack().strength;
                    else
                        total += obj.m_enemy.GetDefense().strength;
                }

                return total;
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
                        AddWounds(); // Unblocked attacks cause wounds
                    else
                        Debug.Log("enemy not defeated");
                }

                m_currentInstance.Disable(); // Disable all enemies in the current instance

                m_combatPanel.m_enemyArea.SelectNext(); // Select the next enemy in line
            }

            void AddWounds()
            {
                Debug.Log("Wounds suffered!");
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
                Game.Manager.Instance.GetCurrentPlayer().AddFame(m_fame);
                Game.Manager.Instance.GetCurrentPlayer().AddReputation(m_reputation);
            }

        }

        public class CombatInstance
        {
            private List<GUI.EnemyHolder> m_enemies;
            private int m_enemyTotal;
            private int m_playerTotal;

            // Constructor for new empty instance
            public CombatInstance()
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

                m_enemyTotal = Combat.Instance.SumEnemies(m_enemies); // Calculate the total strength of enemies in the instance
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
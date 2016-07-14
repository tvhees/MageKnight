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

            public Phase m_phase { get; private set; }

            // Lists to track all enemies in combat and those that have been selected
            private List<Enemy.Object> m_listOfEnemies;
            public Enemy.Band m_band { get; private set; }
            private Players.Strength m_strength;
            private Players.Player m_player;
            private Enemy.Reward m_reward;

            private int woundsThisCombat; // Used to keep track of wounds added to hand THIS combat - for "knocked out" rule

            public IEnumerator StartCombat(Enemy.Object[] newEnemies)
            {
                m_player = Game.Manager.Instance.GetCurrentPlayer();
                m_listOfEnemies = new List<Enemy.Object>(newEnemies); // Store the list of enemies being fought
                m_band = new Enemy.Band(); // Create a new combat instance
                m_reward = Enemy.Reward.NullReward(); // Create a new container for fame + reputation rewards
                m_strength = new Players.Strength(); // Create a new container for player values

                woundsThisCombat = 0; // Reset counter for wounds taken

                m_combatPanel.StartCombat(m_listOfEnemies); // Open the combat UI

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

            IEnumerator PlayerBlock()
            {
                m_phase = Phase.block; // Start the block phase

                while (m_phase == Phase.block)
                {
                    yield return null;
                }
            }

            public bool AddAttackOrBlock(int value, string type = "physical") // Add to a player's total played block or attack
            {
                if (!m_band.IsEmpty())
                {
                    int strength = m_strength.AddStrength(value, type); // Add the attack to the player's status
                    m_combatPanel.m_playerArea.SetStrength(strength, type); // update UI to reflect current strength
                    return true;
                }

                return false; // returns false if there's no selected enemies
            }

            public bool SuccessfulCombat(Players.Strength player, Enemy.Band band)
            {
                int playerTotal = 0;
                int enemyTotal = 0;
                // Block phase rules: Cold attacks require Fire block and vice versa. Ineffective blocks are halved, rounded down
                if (Rules.Combat.Instance.m_phase == Rules.Combat.Phase.block)
                {
                    enemyTotal = band.m_attack.strength;

                    if (band.m_attack.swiftness) // Double the block needed
                        enemyTotal *= 2;

                    if (band.m_attack.cold)
                        player.cold = Mathf.FloorToInt(player.cold / 2f);

                    if (band.m_attack.fire)
                        player.fire = Mathf.FloorToInt(player.fire / 2f);

                    if (band.m_attack.cold || band.m_attack.fire)
                        player.physical = Mathf.FloorToInt(player.fire / 2f);
                }
                // Attack phase rules: Cold attacks halved by Cold resistance, same with Fire.
                else
                {
                    enemyTotal = band.m_defense.strength;

                    if (band.m_defense.physical)
                        player.physical = Mathf.FloorToInt(player.physical / 2f);

                    if (band.m_defense.cold)
                        player.cold = Mathf.FloorToInt(player.cold / 2f);

                    if (band.m_defense.fire)
                        player.cold = Mathf.FloorToInt(player.fire / 2f);

                    if (band.m_defense.cold && band.m_defense.fire)
                        player.coldfire = Mathf.FloorToInt(player.coldfire / 2f);
                }

                playerTotal = player.Total(); // Sum the player's modified values

                return playerTotal >= enemyTotal; // True if the player defeated the enemy in this phase
            }

            public void Resolve()
            {
                if (m_band.IsEmpty()) // If we've pressed this with no enemies selected, move on to the next phase
                {
                    NextPhase();
                    return;
                }
                else if (SuccessfulCombat(m_strength, m_band)) // Otherwise evaluate whether the player has succeeded and proceed accordingly
                {
                    if (m_phase == Phase.block)
                        Debug.Log("attack blocked");
                    else
                        AddRewards(); // This enemy has been killed, at the end of combat we'll get something
                }
                else
                {
                    if (m_phase == Phase.block)
                        woundsThisCombat += m_player.TakeDamage(m_band.m_attack); // Unblocked attacks cause wounds
                    else
                        Debug.Log("enemy not defeated");
                }

                m_band.Disable(); // Disable all enemies in the current instance

                m_combatPanel.m_enemyArea.SelectNext(); // Select the next enemy in line
            }

            void AddRewards()
            {
                // Store the fame and reputation earned from defeating enemie(s)
                m_reward += m_band.m_reward;
                Debug.Log(m_reward.fame + " fame and " + m_reward.reputation + " reputation earned this combat");
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
                m_phase = Phase.end; // End all combat loops
                m_combatPanel.gameObject.SetActive(false); // Turn off the combat UI
                // Give the player combat rewards
                m_player.AddFame(m_reward.fame);
                m_player.AddReputation(m_reward.reputation);
            }
        }
    }
}
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
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

            private Model.TurnState combatState;
            public Phase phase { get; private set; }

            // Lists to track all enemies in combat and those that have been selected
            private List<Enemy.Object> m_listOfEnemies = new List<Enemy.Object>();
            public Enemy.Band band { get; private set; }
            private Players.Strength playerStrength;
            private Players.Player currentPlayer;
            private Enemy.Reward m_reward;

            public int woundsThisCombat; // Used to keep track of wounds added to hand THIS combat - for "knocked out" rule

            public void AddOrRemoveEnemy(Enemy.Object enemy)
            {
                if (m_listOfEnemies.Contains(enemy))
                    m_listOfEnemies.Remove(enemy);
                else
                    m_listOfEnemies.Add(enemy);
            }

            public IEnumerator StartCombat(Model.TurnState combatState)
            {
                this.combatState = combatState;
                currentPlayer = Game.GetCurrentPlayer();
                band = new Enemy.Band(); // Create a new combat instance
                m_reward = Enemy.Reward.NullReward(); // Create a new container for fame + reputation rewards
                playerStrength = new Players.Strength(); // Create a new container for player values

                woundsThisCombat = 0; // Reset counter for wounds taken

                m_combatPanel.StartCombat(m_listOfEnemies); // Open the combat UI

                StartCoroutine(PlayerAttack(Phase.siege)); // Start the first phase
                while (phase != Phase.end) // Don't pass control back to the main game until combat is finished
                {
                    yield return null;
                }
            }

            IEnumerator PlayerAttack(Phase thisPhase)
            {
                

                phase = thisPhase; // Start whichever phase has been called (siege, ranged, attack)

                while (phase == thisPhase) // Stay in this phase until the next has been started
                {
                    yield return null;
                }
            }

            IEnumerator PlayerBlock()
            {
                phase = Phase.block; // Start the block phase

                while (phase == Phase.block)
                {
                    yield return null;
                }
            }

            public bool CanAddAttackOrBlock(int value, string type = "physical") // Add to a player's total played block or attack
            {
                if (!band.IsEmpty())
                {
                    int strength = playerStrength.AddStrength(value, type); // Add the attack to the player's status
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
                if (Instance.phase == Phase.block)
                {
                    enemyTotal = band.attack.strength;

                    if (band.attack.swiftness) // Double the block needed
                        enemyTotal *= 2;

                    if (band.attack.cold)
                        player.cold = Mathf.FloorToInt(player.cold / 2f);

                    if (band.attack.fire)
                        player.fire = Mathf.FloorToInt(player.fire / 2f);

                    if (band.attack.cold || band.attack.fire)
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
                if (band.IsEmpty()) // If we've pressed this with no enemies selected, move on to the next phase
                {
                    NextPhase();
                    return;
                }
                else if (SuccessfulCombat(playerStrength, band)) // Otherwise evaluate whether the player has succeeded and proceed accordingly
                {
                    if (phase == Phase.block) // Block phase - player being attacked
                        Debug.Log("attack blocked");
                    else // Attack phase - player trying to kill enemies
                    {
                        DefeatAndGetRewards(); // This enemy has been killed, at the end of combat we'll get something
                    }
                }
                else
                {
                    if (phase == Phase.block) // Block phase - player being attacked
                        currentPlayer.WoundsDueToAttack(band.attack); // Unblocked attacks cause wounds
                    else
                        Debug.Log("enemy not defeated"); // Attack phase - player trying to kill enemies
                }

                band.Disable(); // Disable all enemies in the current instance

                if (m_combatPanel.m_enemyArea.IsEmpty())
                {
                    EndCombat();
                }
                else
                    m_combatPanel.m_enemyArea.SelectNext(); // Select the next enemy in line
            }

            void DefeatAndGetRewards()
            {
                m_combatPanel.m_enemyArea.DefeatEnemy(band.Enemies());
                
                // Store the fame and reputation earned from defeating enemie(s)
                m_reward += band.m_reward;
                Debug.Log(m_reward.fame + " fame and " + m_reward.reputation + " reputation earned this combat");
            }

            void NextPhase()
            {
                // Move to the next phase of combat based on where we are now
                switch (phase)
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

                // Reset player strength
                playerStrength.Reset();

                // Update UI components
                m_combatPanel.NextPhase();
            }

            void EndCombat()
            {
                phase = Phase.end; // End all combat loops
                m_combatPanel.gameObject.SetActive(false); // Turn off the combat UI
                m_listOfEnemies.Clear(); // Empty the list of enemies

                // Give the player combat rewards
                currentPlayer.AddFame(m_reward.fame);
                currentPlayer.AddReputation(m_reward.reputation);

                // Move the game to the next phase
                combatState.EndCurrentState();
            }
        }
    }
}
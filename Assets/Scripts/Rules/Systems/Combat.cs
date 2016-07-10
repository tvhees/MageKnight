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
            public PhaseIndicator combatPhases;
            public GameObject combatPanel;

            private enum Phase
            {
                siege,
                ranged,
                block,
                attack,
                end
            }

            private Phase phase;

            private List<Enemy.Object> listOfEnemies;
            private bool combatOngoing;

            public IEnumerator StartCombat(Enemy.Object[] newEnemies)
            {
                // Open the combat UI
                combatPanel.SetActive(true);
                combatPhases.StartCombat();

                // Store the list of enemies being fought
                listOfEnemies = new List<Enemy.Object>(newEnemies);

                // Start the first phase
                StartCoroutine(PlayerAttack(Phase.siege));

                while (phase != Phase.end)
                {
                    yield return null;
                }
            }

            IEnumerator PlayerAttack(Phase thisPhase)
            {
                // Start whichever phase has been called. This should also end previous Co-routines;
                phase = thisPhase;

                Debug.Log(listOfEnemies[0].GetDefense().strength);

                while (phase == thisPhase)
                {
                    yield return null;
                }
            }

            IEnumerator PlayerBlock()
            {
                phase = Phase.block;
                Debug.Log(listOfEnemies[0].GetDefense().strength);

                while (phase == Phase.block)
                {
                    yield return null;
                }
            }

            public void Resolve()
            {
                NextPhase();
            }

            void NextPhase()
            {
                combatPhases.NextPhase();

                switch (phase)
                {
                    case Phase.siege:
                        StartCoroutine(PlayerAttack(Phase.ranged));
                        break;
                    case Phase.ranged:
                        StartCoroutine(PlayerBlock());
                        break;
                    case Phase.block:
                        StartCoroutine(PlayerAttack(Phase.attack));
                        break;
                    case Phase.attack:
                        EndCombat();
                        break;
                }
            }

            void EndCombat()
            {
                phase = Phase.end;
                combatPanel.SetActive(false);
                combatOngoing = false;
            }

        }
    }
}
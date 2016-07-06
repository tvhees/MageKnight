using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace BoardGame
{
    namespace Game
    {
        public class Turn : MonoBehaviour
        {
            public enum Phase
            {
                start,
                movement,
                combat,
                influence,
                end,
                cleanup
            }

            [SerializeField]
            private Phase phase;

            public Phase GetPhase()
            {
                return phase;
            }

            public void MoveForward(Phase expectedPhase = Phase.end)
            {
                switch (phase)
                {
                    case Phase.start:
                        phase = expectedPhase;
                        break;
                    case Phase.movement:
                        phase = expectedPhase;
                        break;
                    case Phase.combat:
                    case Phase.influence:
                        phase = Phase.end;
                        break;
                    case Phase.end:
                        phase = Phase.cleanup;
                        break;
                    case Phase.cleanup:
                        StartCoroutine(Cleanup());
                        break;

                }
            }

            public void EndTurn()
            {
                phase = Phase.cleanup;
                MoveForward();
            }

            IEnumerator Cleanup()
            {
                yield return null;

                Manager.Instance.NextPlayer();

                StartCoroutine(NewTurn());
            }

            IEnumerator NewTurn()
            {
                phase = Phase.start;
                yield return null;
            }
        }
    }
}
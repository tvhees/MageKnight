using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace BoardGame
{
    namespace Game
    {
        public class Turn : Singleton<Turn>
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
            private Phase lastPhase;

            public Phase GetPhase()
            {
                return phase;
            }

            public void MoveForward(Phase expectedPhase = Phase.end)
            {
                lastPhase = phase;
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

            public void MoveBackward()
            {
                phase = lastPhase;
            }

            // Check if we can add movement tiles and points
            // i.e. we're in the movement phase or we're in the start phase
            // and can begin the movement phase
            public bool InMovementPhase()
            {
                if (phase == Phase.start)
                    MoveForward(Phase.movement);

                return phase == Phase.movement;
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
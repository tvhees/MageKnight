using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Boardgame
{
    namespace Model
    {
        public class TurnDeprecated : Singleton<Turn>
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

            public void GoToPreviousPhase()
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
                Game.GetCurrentPlayer().EndOfTurn();
                Rules.Movement.Instance.EndMovementPhase();

                yield return null;

                Game.Instance.NextPlayer();

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
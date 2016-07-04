using UnityEngine;
using System.Collections;

namespace BoardGame
{
    namespace Cards
    {
        public class EffectMethods : Singleton<EffectMethods>
        {

            public void RunCoroutine(string methodName, string inputValue)
            {
                StartCoroutine(methodName, inputValue);
            }

            //***********
            // CARD EFFECT METHODS
            //***********

            IEnumerator Movement(string effectValue)
            {
                int value = 0;
                if (int.TryParse(effectValue, out value))
                {
                    Rules.Movement.Instance.AddMovement(value);
                    Manager.Instance.UsedEffect();
                    yield break;
                }

                Manager.Instance.FailedEffect();

                yield return null;
            }

            IEnumerator Attack(string effectValue)
            {
                yield return null;
            }

            IEnumerator Ranged(string effectValue)
            {
                yield return null;
            }

            IEnumerator Influence(string effectValue)
            {
                yield return null;
            }

            IEnumerator Tranquility(string effectValue)
            {
                yield return null;
            }

            IEnumerator Threaten(string effectValue)
            {
                Influence(effectValue);
                // Lose reputation = effectValue

                yield return null;
            }

            IEnumerator Crystallize(string effectValue)
            {
                // Get a mana crystal
                // cost = effectValue

                yield return null;
            }

            IEnumerator ManaDraw(string effectValue)
            {
                // 1 = use additional mana die
                // 2 = set mana die + get two tokens

                yield return null;
            }

            IEnumerator Concentration(string effectiveValue)
            {
                // 1 = gain red/blue/white token
                // 2 = use strong effect with +effectiveValue if move, influence, block, attack

                yield return null;
            }

            IEnumerator Improvisation(string effectiveValue)
            {
                // Discard card for 3/5 of miba

                yield return null;
            }
        }
    }
}
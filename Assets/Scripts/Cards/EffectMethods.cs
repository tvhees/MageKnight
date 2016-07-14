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

            // Elemental card effects
            // Produces one of Movement, Attack, Block, and Influence
            IEnumerator BasicChoice(string effectValue)
            {
                switch (Game.Turn.Instance.GetPhase())
                {
                    case Game.Turn.Phase.movement:
                        StartCoroutine(Movement(effectValue));
                        break;
                    case Game.Turn.Phase.combat:
                        if(Rules.Combat.Instance.m_phase == Rules.Combat.Phase.block)
                            StartCoroutine(Block(effectValue));
                        else
                            StartCoroutine(Attack(effectValue));
                        break;
                    case Game.Turn.Phase.influence:
                        StartCoroutine(Influence(effectValue));
                        break;
                    default:
                        Manager.Instance.FailedEffect();
                        break;
                }

                yield return null;
            }

            IEnumerator Movement(string effectValue)
            {
                if (Game.Turn.Instance.GetPhase() == Game.Turn.Phase.movement)
                {
                    int value = 0;
                    if (int.TryParse(effectValue, out value))
                    {
                        Rules.Movement.Instance.AddMovement(value);
                        Manager.Instance.UsedEffect();
                        yield break;
                    }
                }

                Manager.Instance.FailedEffect();
            }

            // HACKY CODE - could these be condensed? Do I need to pass a special attack class?
            IEnumerator Attack(string effectValue)
            {
                if (Rules.Combat.Instance.m_phase == Rules.Combat.Phase.attack)
                {
                    StartCoroutine(Siege(effectValue));
                }
                else
                    Manager.Instance.FailedEffect();

                yield return null;
            }

            IEnumerator Ranged(string effectValue)
            {
                if (Rules.Combat.Instance.m_phase == Rules.Combat.Phase.attack || Rules.Combat.Instance.m_phase == Rules.Combat.Phase.ranged)
                {
                    StartCoroutine(Siege(effectValue));
                }
                else
                    Manager.Instance.FailedEffect();

                yield return null;
            }

            IEnumerator Siege(string effectValue)
            {
                int value = 0;
                if (int.TryParse(effectValue, out value))
                {
                    if(Rules.Combat.Instance.AddAttackOrBlock(value))
                    {
                        Manager.Instance.UsedEffect();
                        yield break;
                    }
                }

                Manager.Instance.FailedEffect();
            }

            IEnumerator Block(string effectValue)
            {
                int value = 0;
                if (int.TryParse(effectValue, out value))
                {
                    if (Rules.Combat.Instance.m_phase == Rules.Combat.Phase.block)
                    {
                        Rules.Combat.Instance.AddAttackOrBlock(value);
                        Manager.Instance.UsedEffect();
                        yield break;
                    }
                }
                Manager.Instance.FailedEffect();
            }

            IEnumerator Influence(string effectValue)
            {
                yield return null;
            }

            // Basic action cards

            IEnumerator Rage(string effectValue)
            {
                if (Game.Turn.Instance.GetPhase() == Game.Turn.Phase.combat)
                {
                    StartCoroutine(BasicChoice(effectValue));
                    yield break;
                }

                Manager.Instance.FailedEffect();
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

            IEnumerator Concentration(string effectValue)
            {
                // 1 = gain red/blue/white token
                // 2 = use strong effect with +effectiveValue if move, influence, block, attack

                yield return null;
            }

            IEnumerator Improvisation(string effectValue)
            {
                StartCoroutine(BasicChoice(effectValue));

                yield return null;
            }
        }
    }
}
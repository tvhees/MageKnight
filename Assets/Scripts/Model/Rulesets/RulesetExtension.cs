using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Rulesets
{
public abstract class RulesetExtension : MonoBehaviour, Ruleset
	{
        public Ruleset extendedRuleset;

        public virtual void AddMovement(Effect.EffectData input)
        {
            extendedRuleset.AddMovement(input);
        }

        public virtual void AddInfluence(Effect.EffectData input)
        {
            extendedRuleset.AddInfluence(input);
        }

        public virtual void AddAttack(Effect.EffectData input)
        {
            extendedRuleset.AddAttack(input);
        }

        public virtual void AddBlock(Effect.EffectData input)
        {
            extendedRuleset.AddAttack(input);
        }

        public virtual void AddHealing(Effect.EffectData input)
        {
            extendedRuleset.AddHealing(input);
        }

        public virtual void AddOrRemoveEnemyFromCombatSelection(Effect.EffectData input)
        {
            extendedRuleset.AddOrRemoveEnemyFromCombatSelection(input);
        }

        public virtual void StartCombat()
        {
            Debug.Log("Starting Combat");
        }
	}
}
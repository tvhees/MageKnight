using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Rulesets
{
public abstract class RulesetExtension : Ruleset
	{
        public Ruleset extendedRuleset;

        public Commands.Stack CommandStack
        { get { return extendedRuleset.CommandStack; } }

        public Players Players
        { get { return extendedRuleset.Players; } }

        public virtual void AddMovement(EffectData input)
        {
            extendedRuleset.AddMovement(input);
        }

        public virtual void AddInfluence(EffectData input)
        {
            extendedRuleset.AddInfluence(input);
        }

        public virtual void AddAttack(EffectData input)
        {
            extendedRuleset.AddAttack(input);
        }

        public virtual void AddBlock(EffectData input)
        {
            extendedRuleset.AddAttack(input);
        }

        public virtual void AddHealing(EffectData input)
        {
            extendedRuleset.AddHealing(input);
        }

        public virtual void AddOrRemoveEnemyFromCombatSelection(EffectData input)
        {
            extendedRuleset.AddOrRemoveEnemyFromCombatSelection(input);
        }

        public virtual void StartCombat(EffectData input)
        {
            extendedRuleset.StartCombat(input);
        }

        public virtual void Provoke(EffectData input)
        {
            extendedRuleset.Provoke(input);
        }

        public virtual void HexClicked(EffectData input)
        {
            extendedRuleset.HexClicked(input);
        }
    }
}
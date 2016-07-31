using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Boardgame.Rulesets
{
    public class BaseRuleset : MonoBehaviour, Ruleset
    {
        public void AddMovement(Effect.EffectData input)
        {
            Debug.Log("Adding " + input.intValue + " movement points.");
        }

        public void AddInfluence(Effect.EffectData input)
        {
            Debug.Log("Adding " + input.intValue + " influence points.");
        }

        public void AddAttack(Effect.EffectData input)
        {
            Debug.Log("Adding " + input.intValue + " attack points.");
        }

        public void AddBlock(Effect.EffectData input)
        {
            Debug.Log("Adding " + input.intValue + " block points.");
        }

        public void AddHealing(Effect.EffectData input)
        {
            Debug.Log("Adding " + input.intValue + " healing points.");
        }

        public void AddOrRemoveEnemyFromCombatSelection(Effect.EffectData input)
        {
            Debug.Log("Adding or removing " + input.stringValue);
        }

        public void StartCombat()
        {
            Debug.Log("Starting Combat");
        }
    }
}
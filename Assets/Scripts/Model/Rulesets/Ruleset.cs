using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Rulesets
{
    public interface Ruleset : IEventSystemHandler
    {
        void AddMovement(Effect.EffectData input);

        void AddInfluence(Effect.EffectData input);

        void AddAttack(Effect.EffectData input);

        void AddBlock(Effect.EffectData input);

        void AddHealing(Effect.EffectData input);

        void AddOrRemoveEnemyFromCombatSelection(Effect.EffectData input);

        void StartCombat();
    }
}
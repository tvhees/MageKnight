using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Rulesets
{
    public interface Ruleset : IEventSystemHandler
    {
        Commands.Stack CommandStack { get; }

        Players Players { get; }

        void AddMovement(EffectData input);

        void AddInfluence(EffectData input);

        void AddAttack(EffectData input);

        void AddBlock(EffectData input);

        void AddHealing(EffectData input);

        void AddOrRemoveEnemyFromCombatSelection(EffectData input);

        void StartCombat(EffectData input);

        void Provoke(EffectData input);

        void HexClicked(EffectData input);
    }
}
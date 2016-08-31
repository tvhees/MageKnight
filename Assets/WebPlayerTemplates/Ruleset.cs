﻿using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Rulesets
{
    public interface Ruleset : IEventSystemHandler
    {
        Commands.Stack CommandStack { get; }

        Players Players { get; }

        Model.Turn Turn { get;  }

        void AddMovement(EffectData input);

        void AddInfluence(int input);

        void AddAttack(EffectData input);

        void AddBlock(EffectData input);

        void AddHealing(int input, int cost);

        void AddOrRemoveEnemyFromCombatSelection(EffectData input);

        void StartCombat(EffectData input);

        void Provoke(EffectData input);

        void Interact(EffectData input);

        void MoveToTile(EffectData input);

        void UseShop(Board.ShoppingLocation input);
    }
}
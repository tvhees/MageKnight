using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Rulesets
{
public class MovementPhaseRules : RulesetExtension
	{
        public MovementPhaseRules(Ruleset extendedRuleset)
        {
            this.extendedRuleset = extendedRuleset;
            Players.currentPlayer.NewMovementPath();
        }

        public override void Provoke(EffectData effectData)
        {
            float squareDistance = (effectData.positionValue - Players.currentPlayer.position).sqrMagnitude;

            if (Mathf.Sqrt(squareDistance) < GameImpl.unitOfDistance)
            {
                AddOrRemoveEnemyFromCombatSelection(effectData);
            }
        }

        public override void HexClicked(EffectData effectData)
        {
            GameObject tile = effectData.gameObjectValue;
            CommandStack.AddCommand(new AddTileToMovementPath(Players.currentPlayer, tile));
        }
	}
}
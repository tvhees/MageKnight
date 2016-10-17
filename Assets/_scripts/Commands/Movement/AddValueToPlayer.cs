﻿using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System;
using RSG;

namespace Commands
{
    [CreateAssetMenu(menuName = "Command/Add Value To Player")]
    public class AddValueToPlayer : Command
    {
        public GameConstants.ValueType valueType;
        public int valueSize;
        public GameConstants.Element element;
        public UnityAction<int> serverMethod;

        public override void SetInformation(GameData input)
        {
            base.SetInformation(input);
            switch (valueType)
            {
                case GameConstants.ValueType.Movement:
                    serverMethod = gameData.player.ServerAddMovement;
                    break;
                case GameConstants.ValueType.Influence:
                    serverMethod = gameData.player.ServerAddInfluence;
                    break;
                case GameConstants.ValueType.Healing:
                    break;
                case GameConstants.ValueType.Attack:
                    break;
                case GameConstants.ValueType.Block:
                    break;
            }
        }

        public override IEnumerator Routine(Action resolve, Action<Exception> reject)
        {
            serverMethod(valueSize);

            yield return null;

            resolve();
        }

        public override IEnumerator Routine(Action<CommandResult> resolve, Action<Exception> reject)
        {
            serverMethod(valueSize);

            yield return null;

            resolve(CommandResult.success);
        }

        protected override void UndoThisCommand()
        {
            Debug.Log("Undoing");
            serverMethod(-valueSize);
        }
    }
}
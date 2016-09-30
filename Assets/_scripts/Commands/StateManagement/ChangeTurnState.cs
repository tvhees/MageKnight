using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Commands
{
    [CreateAssetMenu(menuName = "Command/Change Turn State")]
    public class ChangeTurnState : Command
    {
        private StateController stateController;
        private GameObject oldState;
        public GameConstants.GameState newState;

        public override void SetInformation(GameData input)
        {
            base.SetInformation(input);
            stateController = GameController.singleton.stateController;

            if (stateController.lastState == null)
                stateController.lastState = stateController.gameState;
            oldState = stateController.lastState;
        }

        protected override CommandResult ExecuteThisCommand()
        {
            stateController.ServerChangeToState(newState);
            return CommandResult.success;
        }

        protected override void UndoThisCommand()
        {
            stateController.ServerChangeToState(oldState);
        }
    }
}
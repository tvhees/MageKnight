using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Commands
{
    public class ChangeTurnState : Command
    {
        private StateController stateController;
        private GameObject oldState;
        private GameObject newState;

        public ChangeTurnState(GameObject newState)
        {
            stateController = GameController.singleton.stateController;
            this.newState = newState;

            if (stateController.lastState == null)
                stateController.lastState = stateController.gameState;
            oldState = stateController.lastState;
        }

        protected override CommandResult ExecuteThisCommand()
        {
            stateController.ServerChangeState(newState);
            return CommandResult.success;
        }

        protected override void UndoThisCommand()
        {
            stateController.ServerChangeState(oldState);
        }
    }
}
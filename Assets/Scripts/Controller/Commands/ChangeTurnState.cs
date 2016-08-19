using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Boardgame
{
    public class ChangeTurnState : Command
    {
        private Model.Turn turn;
        private Model.TurnState oldState;
        private Model.TurnState newState;

        public ChangeTurnState(Model.TurnState newState)
        {
            this.turn = Main.turn;
            this.newState = newState;

            if (turn.backupState == null)
                turn.backupState = turn.currentState;
            oldState = turn.backupState;

            requirements.AddRange(oldState.GetCleanupCommands());
        }

        protected override CommandResult ExecuteThisCommand()
        {
            turn.SetState(newState);
            return CommandResult.success;
        }

        protected override void UndoThisCommand()
        {
            turn.SetState(oldState);
        }
    }
}
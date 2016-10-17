using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Commands
{
    [CreateAssetMenu(menuName = "Command/Change Turn State")]
    public class ChangeTurnState : Command
    {
        StateController stateController;
        GameObject oldState;
        public GameConstants.GameState newState;

        public override void SetInformation(GameData input)
        {
            base.SetInformation(input);
            stateController = GameController.singleton.stateController;

            if (stateController.lastState == null)
                stateController.lastState = stateController.gameState;
            oldState = stateController.lastState;
        }

        public override IEnumerator Routine(Action resolve, Action<Exception> reject)
        {
            yield return null;

            stateController.ChangeToState(newState);
            resolve();
        }

        protected override void UndoThisCommand()
        {
            stateController.ChangeToState(oldState);
        }
    }
}
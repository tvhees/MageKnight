using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RSG;

namespace Commands
{
    public class CommandStack : MonoBehaviour
    {
        public List<Command> oldCommands = new List<Command>();

        public IPromise<GameConstants.Location> Execute(Command command)
        {
            return new Promise<GameConstants.Location>((resolve, reject) => StartCoroutine(command.Routine(resolve, reject)));
        }

        public void UndoLastCommand()
        {
            if (oldCommands.Count > 0)
            {
                Command commandToUndo = oldCommands.GetLast();
                oldCommands.Remove(commandToUndo);
                commandToUndo.Undo();
            }
            else
                Debug.Log("No commands to Undo");

            if (oldCommands.Count <= 0)
                PlayerControl.current.view.RpcEnableUndo(false);
        }

        public void ClearCommandList()
        {
            Debug.Log("Hidden information revealed - Clearing command list");
            oldCommands.Clear();
            PlayerControl.current.view.RpcEnableUndo(false);
        }
    }
}
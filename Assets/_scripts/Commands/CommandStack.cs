using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Commands
{
    public class CommandStack : MonoBehaviour
    {
        public List<Command> oldCommands = new List<Command>();

        public CommandResult RunCommand(Command command)
        {
            CommandResult result = command.Execute();

            if (result.succeeded && result.allowUndo)
            {
                oldCommands.Add(command);
                PlayerControl.current.view.RpcEnableUndo(true);
            }
            else if (result.alternate != null)
                result = RunCommand(result.alternate);

            return result;
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
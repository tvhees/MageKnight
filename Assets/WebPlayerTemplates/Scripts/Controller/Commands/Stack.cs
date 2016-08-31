using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Commands
{
    public class Stack : MonoBehaviour 
	{
        public List<Command> oldCommands = new List<Command>();

        void Awake()
        {
            Main.commandStack = this;
        }

        public CommandResult RunCommand(Command command)
        {
            CommandResult result = command.Execute();

            if (result.succeeded && result.allowUndo)
            {
                oldCommands.Add(command);
            }
            else if (result.alternative != null)
                result = RunCommand(result.alternative);

            Main.turn.backupState = null;

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
        }

        public void ClearCommandList()
        {
            Debug.Log("Hidden information revealed - Clearing command list");
            oldCommands.Clear();
        }
	}
}
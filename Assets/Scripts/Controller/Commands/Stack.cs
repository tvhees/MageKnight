using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Commands
{
    public class Stack : MonoBehaviour 
	{
        public List<Command> oldCommands = new List<Command>();

        public void AddCommand(Command command)
        {
            CommandResult result = command.Execute();

            if (result.succeeded)
            {
                oldCommands.Add(command);
                Debug.Log(command.ToString() + " - total = " + oldCommands.Count);
            }
            else if (result.alternative != null)
                AddCommand(result.alternative);

            Main.turn.backupState = null;
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
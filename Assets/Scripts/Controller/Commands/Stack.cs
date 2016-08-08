using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Commands
{
    public class Stack: MonoBehaviour 
	{
        List<Command> oldCommands = new List<Command>();

        public void AddCommand(Command command)
        {
            if(command.Execute())
                oldCommands.Add(command);
        }

        public void UndoLastCommand()
        {
            if (oldCommands.Count > 0)
            {
                oldCommands.GetLast().Undo();
                oldCommands.RemoveLast();
            }
        }

        public void ClearCommandList()
        {
            Debug.Log("Hidden information revealed - Clearing command list");
            oldCommands.Clear();
        }
	}
}
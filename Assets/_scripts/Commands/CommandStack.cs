using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CommandStack : ScriptableObject
{
    public List<Command> oldCommands = new List<Command>();

    public CommandResult RunCommand(Command command)
    {
        CommandResult result = command.Execute();

        if (result.succeeded && result.allowUndo)
        {
            oldCommands.Add(command);
        }
        else if (result.alternative != null)
            result = RunCommand(result.alternative);

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
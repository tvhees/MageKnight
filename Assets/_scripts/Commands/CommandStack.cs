using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RSG;
using System;
using System.Linq;

namespace Commands
{
    public class CommandStack : MonoBehaviour
    {
        public List<Command> oldCommands = new List<Command>();

        CommandResult sequenceResult;

        #region Running commands
        public void RunCommand(Command command)
        {
            RunCommandWithResult(command);
        }

        public void RunCommandWithoutResult(Command command)
        {
            var sequence = Promise.Sequence(GetSequenceOfPromises(command));
            sequence.Done(() => AddCommand(command));
        }

        Func<IPromise>[] GetSequenceOfPromises(Command command)
        {
            var sequence = new List<Func<IPromise>>();
            for (int i = 0; i < command.instantiatedRequirements.Count; i++)
                sequence.Add(PrepPromise(command.instantiatedRequirements[i]));

            sequence.Add(PrepPromise(command));

            for (int i = 0; i < command.instantiatedOptionals.Count; i++)
                sequence.Add(PrepPromise(command.instantiatedOptionals[i]));

            return sequence.ToArray();
        }

        Func<IPromise> PrepPromise(Command command)
        {
            return () => GetPromiseFromCommand(command);
        }

        IPromise GetPromiseFromCommand(Command command)
        {
            return new Promise((resolve, reject) => StartCoroutine(command.Routine(resolve, reject)));
        }

        #endregion Running commands

        #region Running commands with CommandResult return
        public void RunCommandWithResult(Command mainCommand)
        {
            sequenceResult = CommandResult.success;
            var sequence = Promise.Sequence(GetSequenceOfPromisesWithResult(mainCommand));
            sequence.Done(() => ProcessSequenceResult(mainCommand));
        }

        Func<IPromise>[] GetSequenceOfPromisesWithResult(Command mainCommand)
        {
            var sequence = new List<Func<IPromise>>();
            for (int i = 0; i < mainCommand.instantiatedRequirements.Count; i++)
                sequence.Add(PrepPromiseWithResult(mainCommand, mainCommand.instantiatedRequirements[i]));

            sequence.Add(PrepPromiseWithResult(mainCommand, mainCommand));

            for (int i = 0; i < mainCommand.instantiatedOptionals.Count; i++)
                sequence.Add(PrepPromiseWithResult(mainCommand, mainCommand.instantiatedOptionals[i]));

            return sequence.ToArray();
        }

        Func<IPromise> PrepPromiseWithResult(Command mainCommand, Command subCommand)
        {
            return () => GetPromiseWithResultFromCommand(subCommand)
                .Then(commandResult => ProcessCommandResult(mainCommand, subCommand, commandResult));
        }

        // If a previous command has returned unsuccessful we skip the work in subsequent commands
        IPromise<CommandResult> GetPromiseWithResultFromCommand(Command subCommand)
        {
            return new Promise<CommandResult>((resolve, reject) => 
            {
                if (sequenceResult.succeeded)
                    StartCoroutine(subCommand.Routine(resolve, reject));
                else
                    resolve(sequenceResult);
            });
        }

        // This promise saves the result of the command and returns a dummy, typeless promise that Promise.Sequence can use.
        IPromise ProcessCommandResult(Command mainCommand, Command subCommand, CommandResult commandResult)
        {
            sequenceResult = commandResult;
            if (commandResult.succeeded)
                mainCommand.completedCommands.Add(subCommand);
            return new Promise((resolve, reject) => resolve());
        }

        void ProcessSequenceResult(Command mainCommand)
        {
            if (sequenceResult.CanSaveCommand)
                AddCommand(mainCommand);
            else
            {
                Debug.LogFormat("Can't save command - succeeded: {0}, undoable: {1}", sequenceResult.succeeded, sequenceResult.allowUndo);
                if (!sequenceResult.succeeded)
                {
                    Debug.Log("Undoing failed command: " + mainCommand.name);
                    mainCommand.Undo();
                }
            }
        }

        #endregion Running commands with output

        #region Adding and removing commands

        public void AddCommand(Command command)
        {
            oldCommands.Add(command);
            PlayerControl.current.view.RpcEnableUndo(true);
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

        #endregion Adding and removing commands
    }
}
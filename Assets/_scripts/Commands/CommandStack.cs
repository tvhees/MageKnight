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

        CommandResult sequenceResult;

        public void RunCommandWithResult(Command command)
        {
            sequenceResult = CommandResult.success;
            var sequence = Promise.Sequence(GetSequenceOfPromisesWithResult(command));
            sequence.Done(() => ProcessSequenceResult(command));
        }

        Func<IPromise>[] GetSequenceOfPromisesWithResult(Command command)
        {
            var sequence = new List<Func<IPromise>>();
            for (int i = 0; i < command.instantiatedRequirements.Count; i++)
                sequence.Add(PrepPromiseWithResult(command.instantiatedRequirements[i]));

            sequence.Add(PrepPromiseWithResult(command));

            for (int i = 0; i < command.instantiatedOptionals.Count; i++)
                sequence.Add(PrepPromiseWithResult(command.instantiatedOptionals[i]));

            return sequence.ToArray();
        }

        Func<IPromise> PrepPromiseWithResult(Command command)
        {
            return () => GetPromiseWithResultFromCommand(command)
                .Then(commandResult => ProcessCommandResult(commandResult));
        }

        // If a previous command has returned unsuccessful we skip the work in subsequent commands
        IPromise<CommandResult> GetPromiseWithResultFromCommand(Command command)
        {
            return new Promise<CommandResult>((resolve, reject) => 
            {
                if (sequenceResult.succeeded)
                    StartCoroutine(command.Routine(resolve, reject));
                else
                    resolve(sequenceResult);
            });
        }

        // This promise saves the result of the command and returns a dummy, typeless promise that Promise.Sequence can use.
        IPromise ProcessCommandResult(CommandResult commandResult)
        {
            sequenceResult = commandResult;
            Debug.Log(commandResult.succeeded);
            return new Promise((resolve, reject) => resolve());
        }

        void ProcessSequenceResult(Command command)
        {
            if (sequenceResult.CanSaveCommand)
                AddCommand(command);
            else
                Debug.LogFormat("Can't save command - succeeded: {0}, undoable: {1}", sequenceResult.succeeded, sequenceResult.allowUndo);
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
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using RSG;

namespace Commands
{
    public abstract class Command : ScriptableObject
    {
        protected GameData gameData;

        public Command[] requirements;
        [HideInInspector]
        public List<Command> instantiatedRequirements = new List<Command>();

        public Command[] optionals;
        [HideInInspector]
        public List<Command> instantiatedOptionals = new List<Command>();

        [HideInInspector]
        public List<Command> completedCommands = new List<Command>();

        public virtual IEnumerator Routine(Action<CommandResult> resolve, Action<Exception> reject)
        {
            yield return null;

            reject(new ApplicationException("Command Not Implemented"));
        }

        public virtual void UndoThisCommand()
        {
            Debug.LogFormat("The {0} command has no undo function", name);
        }

        public virtual void SetInformation(GameData input)
        {
            gameData = input;

            for (int i = 0; i < requirements.Length; i++)
            {
                var command = Instantiate(requirements[i]);
                command.SetInformation(input);
                instantiatedRequirements.Add(command);
            }

            for (int i = 0; i < optionals.Length; i++)
            {
                var command = Instantiate(optionals[i]);
                command.SetInformation(input);
                instantiatedOptionals.Add(command);
            }
        }

        public void Undo()
        {
            for (int i = completedCommands.Count - 1; i >= 0; i--)
                completedCommands[i].UndoThisCommand();

            completedCommands.Clear();
        }
    }
}
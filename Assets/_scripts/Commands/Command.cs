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
        protected List<Command> completedRequirements = new List<Command>();

        public Command[] optionals;
        [HideInInspector]
        public List<Command> instantiatedOptionals = new List<Command>();
        protected List<Command> completedOptionals = new List<Command>();

        public virtual IEnumerator Routine(Action resolve, Action<Exception> reject)
        {
            yield return null;

            reject(new ApplicationException("Command Not Implemented"));
        }

        public virtual IEnumerator Routine(Action<CommandResult> resolve, Action<Exception> reject)
        {
            yield return null;

            var cResult = CommandResult.success;

            resolve(cResult);
        }

        protected virtual void UndoThisCommand()
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
            UndoCompletedOptionals();
            UndoThisCommand();
            UndoCompletedRequirements();
        }

        protected void UndoCompletedOptionals()
        {
            for (int i = completedOptionals.Count; i > 0; i--)
            {
                completedOptionals.GetLast(true).Undo();
            }
        }

        protected void UndoCompletedRequirements()
        {
            for (int i = completedRequirements.Count; i > 0; i--)
            {
                completedRequirements.GetLast(true).Undo();
            }
        }
    }
}
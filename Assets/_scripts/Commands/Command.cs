using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


namespace Commands
{
    public abstract class Command : ScriptableObject
    {
        protected GameData gameData;

        public List<Command> requirements;
        protected List<Command> instantiatedRequirements = new List<Command>();
        protected List<Command> completedRequirements = new List<Command>();

        public List<Command> optionals;
        protected List<Command> instantiatedOptionals = new List<Command>();
        protected List<Command> completedOptionals = new List<Command>();

        public virtual IEnumerator Routine(Action<GameConstants.Location> resolve, Action<Exception> reject)
        {
            yield return null;

            reject(new ApplicationException("Command Not Implemented"));
        }

        protected virtual void UndoThisCommand()
        {
            Debug.LogFormat("The {0} command has no undo function", name);
        }

        public virtual void SetInformation(GameData input)
        {
            gameData = input;

            for (int i = 0; i < requirements.Count; i++)
            {
                var command = Instantiate(requirements[i]);
                command.SetInformation(input);
                instantiatedRequirements.Add(command);
            }

            for (int i = 0; i < optionals.Count; i++)
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
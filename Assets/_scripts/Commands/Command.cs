using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Commands;


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

        public Command alternate ;
        Command instantiatedAlternate;

        protected abstract CommandResult ExecuteThisCommand();

        protected virtual void UndoThisCommand()
        {

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

            if (alternate != null)
            {
                instantiatedAlternate = Instantiate(alternate);
                instantiatedAlternate.SetInformation(input);
            }
        }

        public CommandResult Execute()
        {
            var result = ExecuteRequiredCommands();

            if (result.succeeded)
            {
                result = ExecuteThisCommand();
                if (result.succeeded)
                    ExecuteOptionalCommands();
            }

            if (!result.succeeded)
            {
                UndoCompletedRequirements();
                result = CommandResult.failure;
                result.alternate = instantiatedAlternate;
            }

            return result;
        }

        protected CommandResult ExecuteRequiredCommands()
        {
            var result = CommandResult.success;
            for (int i = 0; i < instantiatedRequirements.Count; i++)
            {
                result = ExecuteSubCommand(instantiatedRequirements[i], completedRequirements);
                if (!result.succeeded)
                {
                    UndoCompletedRequirements();
                    break;
                }
            }
            return result;
        }

        protected void ExecuteOptionalCommands()
        {
            for (int i = 0; i < instantiatedOptionals.Count; i++)
            {
                ExecuteSubCommand(instantiatedOptionals[i], completedOptionals);
            }
        }

        protected CommandResult ExecuteSubCommand(Command subCommand, List<Command> completedSubCommandList)
        {
            var result = subCommand.Execute();
            if (result.succeeded)
                completedSubCommandList.Add(subCommand);
            return result;
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
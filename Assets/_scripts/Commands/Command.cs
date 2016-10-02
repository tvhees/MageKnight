﻿using UnityEngine;
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

        public Command alternate = null;
        private Command instantiatedAlternate = null;

        protected abstract CommandResult ExecuteThisCommand();

        protected abstract void UndoThisCommand();

        public virtual void SetInformation(GameData input)
        {
            gameData = input;

            for (int i = 0; i < requirements.Count; i++)
            {
                Command command = Instantiate(requirements[i]);
                command.SetInformation(input);
                instantiatedRequirements.Add(command);
            }

            for (int i = 0; i < optionals.Count; i++)
            {
                Command command = Instantiate(optionals[i]);
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
            CommandResult result = ExecuteRequiredCommands();

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
            CommandResult result = CommandResult.success;
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
            CommandResult result = subCommand.Execute();
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
                completedOptionals.GetLast(remove: true).Undo();
            }
        }

        protected void UndoCompletedRequirements()
        {
            for (int i = completedRequirements.Count; i > 0; i--)
            {
                completedRequirements.GetLast(remove: true).Undo();
            }
        }
    }
}
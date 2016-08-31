﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
    public abstract class Command 
	{
        protected List<Command> requirements = new List<Command>();
        protected List<Command> completedRequirements = new List<Command>();

        protected List<Command> optionals = new List<Command>();
        protected List<Command> completedOptionals = new List<Command>();

        protected abstract CommandResult ExecuteThisCommand();

        protected abstract void UndoThisCommand();

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
                UndoCompletedRequirements();

            return result;
        }

        protected CommandResult ExecuteRequiredCommands()
        {
            CommandResult result = CommandResult.success;
            for (int i = 0; i < requirements.Count; i++)
            {
                result = ExecuteSubCommand(requirements[i], completedRequirements);
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
            for (int i = 0; i < optionals.Count; i++)
            {
                ExecuteSubCommand(optionals[i], completedOptionals);
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
                completedOptionals.GetLast().Undo();
                completedOptionals.RemoveLast();
            }
        }

        protected void UndoCompletedRequirements()
        {
            for (int i = completedRequirements.Count; i > 0; i--)
            {
                completedRequirements.GetLast().Undo();
                completedRequirements.RemoveLast();
            }
        }
    }
}
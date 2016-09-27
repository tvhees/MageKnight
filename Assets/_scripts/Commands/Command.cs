using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Commands
{
    public abstract class Command : ScriptableObject
    {
        protected PlayerControl player;

        protected List<Command> requirements = new List<Command>();
        protected List<Command> completedRequirements = new List<Command>();

        protected List<Command> optionals = new List<Command>();
        protected List<Command> completedOptionals = new List<Command>();

        protected abstract CommandResult ExecuteThisCommand();

        protected abstract void UndoThisCommand();

        public CommandResult Execute(PlayerControl player)
        {
            this.player = player;

            int i = 0;

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
            CommandResult result = subCommand.Execute(player);
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
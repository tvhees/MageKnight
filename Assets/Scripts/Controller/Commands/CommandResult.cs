using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
    public class CommandResult
    {
        public static CommandResult success = new CommandResult(true);
        public static CommandResult failure = new CommandResult(false);

        public bool succeeded;
        public Command alternative;

        public CommandResult(Command alternative)
        {
            succeeded = false;
            this.alternative = alternative;
        }

        public CommandResult(bool succeeded)
        {
            this.succeeded = succeeded;
            alternative = null;
        }
    }
}
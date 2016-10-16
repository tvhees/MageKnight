using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Commands
{
    public struct CommandResult
    {
        public static CommandResult success = new CommandResult(true);
        public static CommandResult failure = new CommandResult(false);
        public static CommandResult permanent = new CommandResult(true, false);

        public bool succeeded;
        public bool allowUndo;

        public CommandResult(bool succeeded, bool allowUndo = true)
        {
            this.succeeded = succeeded;
            this.allowUndo = allowUndo;
        }
    }
}
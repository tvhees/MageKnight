using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
    public abstract class Command 
	{
        public abstract bool Execute();

        public abstract void Undo();
	}
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
    public interface Attack
	{
        int Value { get; set; }

        bool[] Attributes { get; set; }
	}
}
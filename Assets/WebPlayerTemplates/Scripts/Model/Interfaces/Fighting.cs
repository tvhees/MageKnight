using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
    public interface Fighting 
	{
        bool TestDefense(Attack incoming);

        Attack GetAttack();
	}
}
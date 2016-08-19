using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Boardgame.Enemy
{
    public class Orc : Enemy 
	{
        private Attack attack;
        private Defense defense;

        public override Attack GetAttack()
        {
            Debug.Log("Return this enemy's attack");
            return attack;
        }

        public override bool TestDefense(Attack input)
        {
            Debug.Log("See if this enemy's defense was beaten by the player");
            return true;
        }
    }
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Boardgame.Enemy;
using System;

namespace Boardgame
{
    public class EnemyStore : ComponentStore
    {
        protected override Component CreateComponent(string[] input)
        {
            Component component = null;
            factory = new EnemyFactory(input[0]);

            if (input[0].Equals("Orc"))
                component = new Orc(input[1], factory);

            return component;
        }
    }
}
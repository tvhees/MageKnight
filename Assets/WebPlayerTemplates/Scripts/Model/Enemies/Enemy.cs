using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Boardgame.Enemy
{
    public abstract class Enemy : MonoBehaviour, Fighting
    {
        public abstract bool TestDefense(Attack input);
        public abstract Attack GetAttack();
    }
}
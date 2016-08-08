using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Effect
{
    public abstract class BaseEffect : MonoBehaviour
    {
        public int intValue;
        public EffectData effectData;

        public abstract void UseEffect(Rulesets.Ruleset rules);
    }
}
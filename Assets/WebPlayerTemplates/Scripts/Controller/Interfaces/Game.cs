using UnityEngine;

namespace Boardgame
{
    interface Game
    {
        Rulesets.BaseRuleset Rules { get; }
    }
}

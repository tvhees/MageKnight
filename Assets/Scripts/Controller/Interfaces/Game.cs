using UnityEngine;

namespace Boardgame
{
    interface Game
    {
        Rulesets.Ruleset Rules { get; }
    }
}

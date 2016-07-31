using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
    namespace Building
    {
        public class Spoils : MonoBehaviour
        {
            public void GetReward(Rulesets.Components.Feature feature)
            {
                switch (feature)
                {
                    case Rulesets.Components.Feature.village:
                        // Draw two cards
                        break;
                    case Rulesets.Components.Feature.tower:
                        // Spell from spell offer
                        break;
                    case Rulesets.Components.Feature.monastery:
                        // Artifact
                        break;
                    case Rulesets.Components.Feature.den:
                        // 2 x Roll die for mana crystal
                        break;
                    case Rulesets.Components.Feature.dungeon:
                        // Roll die for spell or artifact
                        break;
                    case Rulesets.Components.Feature.spawning:
                        // Artifact + 3 x Roll die for mana crystal
                        break;
                    case Rulesets.Components.Feature.tomb:
                        // 1 spell + 1 artifact
                        break;
                }
            }
        }
    }
}
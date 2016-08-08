using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
    namespace Building
    {
        public class Spoils : MonoBehaviour
        {
            public void GetReward(Rules.Components.Feature feature)
            {
                switch (feature)
                {
                    case Rules.Components.Feature.village:
                        // Draw two cards
                        break;
                    case Rules.Components.Feature.tower:
                        // Spell from spell offer
                        break;
                    case Rules.Components.Feature.monastery:
                        // Artifact
                        break;
                    case Rules.Components.Feature.den:
                        // 2 x Roll die for mana crystal
                        break;
                    case Rules.Components.Feature.dungeon:
                        // Roll die for spell or artifact
                        break;
                    case Rules.Components.Feature.spawning:
                        // Artifact + 3 x Roll die for mana crystal
                        break;
                    case Rules.Components.Feature.tomb:
                        // 1 spell + 1 artifact
                        break;
                }
            }
        }
    }
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
    namespace Board
    {
        public class ScenarioDatabase : ScriptableObjectDatabase<Scenario>
        {
            void Awake()
            {
                path = "Scenarios";
            }
        }
    }
}
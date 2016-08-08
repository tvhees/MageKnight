using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Boardgame
{
	namespace Board
    {
		public class  ScenarioTester : ScriptableObjectTester<Scenario>
        {
            protected override void LogDescription(Scenario obj)
            {
                Debug.Log(string.Format("Name: {0}, Description: {1}", obj.name, obj.description));
            }
        }
	}
}
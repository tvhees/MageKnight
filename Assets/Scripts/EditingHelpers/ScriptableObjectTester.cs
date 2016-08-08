using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
	namespace Board
    {
		public abstract class ScriptableObjectTester<T> : MonoBehaviour where T : ScriptableObject
		{
            public string objName;

            void Start()
            {
                ScriptableObjectDatabase<T> db = GetComponent<ScriptableObjectDatabase<T>>();
                T obj = db.GetScriptableObject(objName);
                if (obj != null)
                    LogDescription(obj);
            }

            protected abstract void LogDescription(T obj);
		}
	}
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
    public abstract class ScriptableObjectDatabase<T> : MonoBehaviour where T : ScriptableObject
    {
        protected string path;

        protected Dictionary<string, T> scriptableObjects;

        protected bool databaseIsLoaded = false;

        protected void ValidateDatabase()
        {
            if (scriptableObjects == null) scriptableObjects = new Dictionary<string, T>();
            if (!databaseIsLoaded) LoadDatabase();
        }

        public void LoadDatabase()
        {
            if (databaseIsLoaded) return;
            databaseIsLoaded = true;
            LoadDatabaseForce();
        }

        public void LoadDatabaseForce()
        {
            scriptableObjects.Clear();
            ValidateDatabase();
            T[] resources = Resources.LoadAll<T>(@path);
            foreach (var sObject in resources)
            {
                if (!scriptableObjects.ContainsValue(sObject)) scriptableObjects.Add(sObject.name, sObject);
            }
        }

        public void ClearDatabase()
        {
            databaseIsLoaded = false;
            scriptableObjects.Clear();
        }

        public T GetScriptableObject(string name)
        {
            ValidateDatabase();

            T scriptableObject;
            if (scriptableObjects.TryGetValue(name, out scriptableObject)) return ScriptableObject.Instantiate(scriptableObject) as T;
            else return default(T);
        }

        public T[] GetAllObjects()
        {
            ValidateDatabase();
            T[] valueArray = new T[scriptableObjects.Count];
            scriptableObjects.Values.CopyTo(valueArray, 0);
            return valueArray;
        }
    }
}
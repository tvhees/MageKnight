using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Other.Data;

namespace Other.Utility
{
    public static class ScenarioDatabase
    {
        public const string path = "Scenarios";

        private static Dictionary<string, Scenario> scriptableObjects;

        private static bool databaseIsLoaded = false;

        private static void ValidateDatabase()
        {
            if (scriptableObjects == null) scriptableObjects = new Dictionary<string, Scenario>();
            if (!databaseIsLoaded) LoadDatabase();
        }

        private static void LoadDatabase()
        {
            if (databaseIsLoaded) return;
            databaseIsLoaded = true;
            LoadDatabaseForce();
        }

        private static void LoadDatabaseForce()
        {
            scriptableObjects.Clear();
            ValidateDatabase();
            Scenario[] resources = Resources.LoadAll<Scenario>(@path);
            foreach (var sObject in resources)
            {
                if (!scriptableObjects.ContainsValue(sObject)) scriptableObjects.Add(sObject.name, sObject);
            }
        }

        private static void ClearDatabase()
        {
            databaseIsLoaded = false;
            scriptableObjects.Clear();
        }

        public static Scenario GetScriptableObject(string name)
        {
            ValidateDatabase();

            Scenario scriptableObject;
            if (scriptableObjects.TryGetValue(name, out scriptableObject)) return ScriptableObject.Instantiate(scriptableObject) as Scenario;
            else return default(Scenario);
        }

        public static Scenario[] GetAllObjects()
        {
            ValidateDatabase();
            Scenario[] valueArray = new Scenario[scriptableObjects.Count];
            scriptableObjects.Values.CopyTo(valueArray, 0);
            return valueArray;
        }
    }
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Other.Data;

namespace Other.Utility
{
    public static class CharacterDatabase
    {
        public const string path = "Characters";

        private static Dictionary<string, Character> scriptableObjects;

        private static bool databaseIsLoaded = false;

        private static void ValidateDatabase()
        {
            if (scriptableObjects == null) scriptableObjects = new Dictionary<string, Character>();
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
            Character[] resources = Resources.LoadAll<Character>(@path);
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

        public static Character GetScriptableObject(string name)
        {
            ValidateDatabase();

            Character scriptableObject;
            if (scriptableObjects.TryGetValue(name, out scriptableObject)) return ScriptableObject.Instantiate(scriptableObject) as Character;
            else return default(Character);
        }

        public static Character[] GetAllObjects()
        {
            ValidateDatabase();
            Character[] valueArray = new Character[scriptableObjects.Count];
            scriptableObjects.Values.CopyTo(valueArray, 0);
            return valueArray;
        }
    }
}
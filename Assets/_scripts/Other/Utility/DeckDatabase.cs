using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Other.Data;

namespace Other.Utility
{
    public static class DeckDatabase
    {
        public const string path = "Decks";

        private static Dictionary<string, Deck> scriptableObjects;

        private static bool databaseIsLoaded = false;

        private static void ValidateDatabase()
        {
            if (scriptableObjects == null) scriptableObjects = new Dictionary<string, Deck>();
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
            Deck[] resources = Resources.LoadAll<Deck>(@path);
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

        public static Deck GetScriptableObject(string name)
        {
            ValidateDatabase();

            Deck scriptableObject;
            if (scriptableObjects.TryGetValue(name, out scriptableObject)) return ScriptableObject.Instantiate(scriptableObject) as Deck;
            else return default(Deck);
        }

        public static Deck[] GetAllObjects()
        {
            ValidateDatabase();
            Deck[] valueArray = new Deck[scriptableObjects.Count];
            scriptableObjects.Values.CopyTo(valueArray, 0);
            return valueArray;
        }
    }
}
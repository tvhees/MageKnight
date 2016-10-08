using Other.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Other.Utility
{
    public static class CardDatabase
    {
        public const string path = "Cards";

        static Dictionary<string, Card> scriptableObjects;

        static bool databaseIsLoaded;

        static void ValidateDatabase()
        {
            if (scriptableObjects == null) scriptableObjects = new Dictionary<string, Card>();
            if (!databaseIsLoaded) LoadDatabase();
        }

        static void LoadDatabase()
        {
            if (databaseIsLoaded) return;
            databaseIsLoaded = true;
            LoadDatabaseForce();
        }

        static void LoadDatabaseForce()
        {
            ClearDatabase();
            ValidateDatabase();
            var resources = Resources.LoadAll<Card>(@path);
            foreach (var sObject in resources)
            {
                if (!scriptableObjects.ContainsValue(sObject)) scriptableObjects.Add(sObject.name, sObject);
            }
        }

        static void ClearDatabase()
        {
            databaseIsLoaded = false;
            scriptableObjects.Clear();
        }

        public static Card GetScriptableObject(string name)
        {
            ValidateDatabase();
            Card scriptableObject;
            return scriptableObject = scriptableObjects.TryGetValue(name, out scriptableObject) ? Object.Instantiate(scriptableObject) as Card : default(Card);
        }

        public static Card[] GetAllObjects()
        {
            ValidateDatabase();
            var valueArray = new Card[scriptableObjects.Count];
            scriptableObjects.Values.CopyTo(valueArray, 0);
            return valueArray;
        }
    }
}
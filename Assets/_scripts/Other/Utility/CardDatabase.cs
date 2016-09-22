using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Other.Data;

namespace Other.Utility
{
    public static class CardDatabase
    {
        public const string path = "Cards";

        private static Dictionary<string, Card> scriptableObjects;

        private static bool databaseIsLoaded = false;

        private static void ValidateDatabase()
        {
            if (scriptableObjects == null) scriptableObjects = new Dictionary<string, Card>();
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
            Card[] resources = Resources.LoadAll<Card>(@path);
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

        public static Card GetScriptableObject(string name)
        {
            ValidateDatabase();

            Card scriptableObject;
            if (scriptableObjects.TryGetValue(name, out scriptableObject)) return ScriptableObject.Instantiate(scriptableObject) as Card;
            else return default(Card);
        }

        public static Card[] GetAllObjects()
        {
            ValidateDatabase();
            Card[] valueArray = new Card[scriptableObjects.Count];
            scriptableObjects.Values.CopyTo(valueArray, 0);
            return valueArray;
        }
    }
}
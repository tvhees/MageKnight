using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Other.Data;

namespace Other.Utility
{
    public static class TileDatabase
    {
        public const string path = "Tiles";

        private static Dictionary<string, HexTile> scriptableObjects;

        private static bool databaseIsLoaded = false;

        private static void ValidateDatabase()
        {
            if (scriptableObjects == null) scriptableObjects = new Dictionary<string, HexTile>();
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
            HexTile[] resources = Resources.LoadAll<HexTile>(@path);
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

        public static HexTile GetScriptableObject(string name)
        {
            ValidateDatabase();

            HexTile scriptableObject;
            if (scriptableObjects.TryGetValue(name, out scriptableObject)) return ScriptableObject.Instantiate(scriptableObject) as HexTile;
            else return default(HexTile);
        }

        public static HexTile[] GetAllObjects()
        {
            ValidateDatabase();
            HexTile[] valueArray = new HexTile[scriptableObjects.Count];
            scriptableObjects.Values.CopyTo(valueArray, 0);
            return valueArray;
        }
    }
}
using Other.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Other.Utility
{
    public static class MapShapeDatabase
    {
        public const string path = "MapShapes";

        private static Dictionary<string, MapShape> scriptableObjects;

        private static bool databaseIsLoaded = false;

        private static void ValidateDatabase()
        {
            if (scriptableObjects == null) scriptableObjects = new Dictionary<string, MapShape>();
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
            MapShape[] resources = Resources.LoadAll<MapShape>(@path);
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

        public static MapShape GetScriptableObject(string name)
        {
            ValidateDatabase();

            MapShape scriptableObject;
            if (scriptableObjects.TryGetValue(name, out scriptableObject)) return ScriptableObject.Instantiate(scriptableObject) as MapShape;
            else return default(MapShape);
        }

        public static MapShape[] GetAllObjects()
        {
            ValidateDatabase();
            MapShape[] valueArray = new MapShape[scriptableObjects.Count];
            scriptableObjects.Values.CopyTo(valueArray, 0);
            return valueArray;
        }
    }
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// This class contains methods for creating an object pool
// we can then re-use objects from the pool instead of instantiating new clones
// Multiple prefabs can be used, be aware that if the pool size is not divisible
// by the number of different prefabs then some prefabs will have one less 

namespace Boardgame
{
    public class ObjectPool : MonoBehaviour
    {

        // This is the GameObject we want to make a pool of
        public GameObject[] prefabs;

        // Should the pool automatically be created on awake and how large should it be?
        public bool createOnAwake;
        public int defaultSize;

        // Position that objects are kept at while in the pool
        protected Vector3 homePosition;

        // Lists to keep track of objects in the pool and objects in use
        protected List<GameObject> pool;
        protected List<GameObject> checkedOut;

        // We want to be able to find out the number of available objects from outside the class
        public int available
        {
            get { return pool.Count; }
        }

        void Awake()
        {
            if (createOnAwake)
                CreatePool(defaultSize);
        }

        // Make a new pool. Objects are set as inactive children of the pool. 
        public void CreatePool(int size)
        {
            if (pool == null)
            {
                pool = new List<GameObject>();
                checkedOut = new List<GameObject>();

                for (int i = 0; i < size; i++)
                {
                    // Create a new index based on i to make sure we don't exceed the prefabs array length
                    int j = (int)Mathf.Repeat(i, prefabs.Length);

                    GameObject obj = transform.InstantiateChild(prefabs[j], homePosition);
                    obj.SetActive(false);
                    pool.Add(obj);
                }
            }
            else
            {
                // We don't want to call this on an existing pool as we would lose
                // track of the old objects
                Debug.Log("Pool already exists, preventing overwrite");
            }
        }

        // Gets an available object in the pool and activates it
        public GameObject GetObject(bool random = false)
        {
            // By default we take the first available object from the pool
            int index = 0;
            if (random)
                index = Random.Range(0, available);

            return GetObject(index);
        }

        // Method overload; sGet a specifically named object from the pool
        public GameObject GetObject(string searchName)
        {
            for (int i = 0; i < available; i++)
            {
                if (pool[i].name == searchName)
                    return GetObject(i);
            }

            Debug.Log("No object with the name \'" + searchName + "\' found in pool");

            return null;
        }


        // Method overload; Get an object with a specific index
        public GameObject GetObject(int index)
        {
            // Debug Code
            if (pool == null)
                Debug.Log("Pool doesn't exist");
            else if (available < 1)
                Debug.Log("Pool is empty, all objects in use");
            else if (index < 0 || index >= available)
                Debug.Log("Index outside of valid range");
            else
            {
                GameObject obj = pool[index];

                // Move the object from the pool list to the in use list and make it active
                pool.Remove(obj);
                checkedOut.Add(obj);
                obj.SetActive(true);

                return obj;
            }

            return null;
        }

        // Sends an object back to the pool and de-activates it
        public void ReturnObject(GameObject obj)
        {
            pool.Add(obj);
            checkedOut.Remove(obj);
            obj.transform.SetParent(transform);
            obj.transform.position = homePosition;
            obj.transform.localScale = Vector3.one;
            obj.SetActive(false);
        }

        // Bring ALL objects back to the pool
        public void Reset()
        {
            while (checkedOut.Count > 0)
            {
                ReturnObject(checkedOut[0]);
            }
        }
    }
}
﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ExtensionMethods
{

    /// <summary>
    /// Randomises and array
    /// </summary>
    public static T[] Randomise<T>(this T[] param)
    {
        for (int i = 0; i < param.Length; i++)
        {
            int j = Random.Range(i, param.Length);
            var temp = param[i];
            param[i] = param[j];
            param[j] = temp;
        }
        return param;
    }

    /// <summary>
    /// Randomises a list
    /// </summary>
    public static List<T> Randomise<T>(this List<T> param, bool createCopy = true)
    {
        for (int i = 0; i < param.Count; i++)
        {
            int j = Random.Range(i, param.Count);
            var temp = param[i];
            param[i] = param[j];
            param[j] = temp;
        }
        return param;
    }


    /// <summary>
    /// Adds an integer range to a list.
    /// </summary>
    /// <param name="param"></param>
    /// <param name="start"></param>
    /// <param name="end"></param>
    public static void AddIntRange(this List<int> param, int start, int end)
    {
        for (int i = start; i <= end; i++)
            param.Add(i);
    }

    public static T GetLast<T>(this List<T> param)
    {
        T lastItem;
        try
        {
            lastItem = param[param.Count - 1];
            return lastItem;
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("Error getting last item: " + e);
            return default(T);
        }
    }

    public static void RemoveLast<T>(this List<T> param)
    {
        int i;
        try
        {
            i = param.Count - 1;
            param.RemoveAt(i);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("Error removing last item: " + e);
        }
    }

    /// <summary>
    /// Instantiates a new GameObject as a child of the calling transform.
    /// </summary>
    public static GameObject InstantiateChild(this Transform parent, GameObject obj)
    {
        GameObject instance = Object.Instantiate(obj, parent.transform.position, Quaternion.identity) as GameObject;
        instance.transform.SetParent(parent);
        return instance;
    }

    /// <summary>
    /// Instantiates a new GameObject as a child of the calling transform, with specific position and rotation.
    /// </summary>
    public static GameObject InstantiateChild(this Transform parent, GameObject obj, Vector3 localPosition, Quaternion localRotation = default(Quaternion))
    {
        GameObject instance = Object.Instantiate(obj);
        instance.transform.SetParent(parent);
        instance.transform.localPosition = localPosition;
        instance.transform.localRotation = localRotation;
        return instance;
    }

    /// <summary>
    /// Returns the last child of a transform by heirarchy index
    /// </summary>
    public static GameObject LastChild(this Transform parent)
    {
        if (parent.childCount > 0)
        {
            int index = parent.childCount - 1;
            return parent.GetChild(index).gameObject;
        }
        else
            return null;
    }

    /// <summary>
    /// Gets or add a component. Required for Singleton pattern. Usage example:
    /// BoxCollider boxCollider = transform.GetOrAddComponent<BoxCollider>();
    /// </summary>
    public static T GetOrAddComponent<T>(this Component child) where T : Component
    {
        T result = child.GetComponent<T>();
        if (result == null)
        {
            result = child.gameObject.AddComponent<T>();
        }
        return result;
    }

    /// <summary>
    /// Bitshifts a layer's index to give the correct layerMask;
    /// </summary>
    public static LayerMask IntToLayer(this LayerMask layerMask, int layer)
    {
        layerMask = 1 << layer;
        return layerMask;
    }

    /// <summary>
    /// Move a Rect current towards a target
    /// <summary>
    public static Rect MoveTowards(this Rect current, Rect target, float maxDistanceDelta)
    {
        current.position = Vector2.MoveTowards(current.position, target.position, maxDistanceDelta);
        current.size = Vector2.MoveTowards(current.size, target.size, maxDistanceDelta);

        return current;
    }

    /// <summary>
    /// Resets a RectTransform component to zero offset and uniform scale of 1
    /// </summary>
    /// <param name="param"></param>
    public static void Reset(this RectTransform param)
    {
        param.offsetMax = Vector2.zero;
        param.offsetMin = Vector2.zero;
        param.localScale = Vector3.one;
    }
}
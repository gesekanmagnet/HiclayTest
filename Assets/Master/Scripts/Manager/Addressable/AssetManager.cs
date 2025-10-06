using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public static class AssetManager
{
    private static List<GameObject> instances = new();
    private static List<AsyncOperationHandle> handles = new();
    private static Dictionary<string, Object> assets = new();

    private static void Register(string key, Object value)
    {
        if(!assets.ContainsKey(key))
        {
            assets.Add(key, value);
        }
        else
            assets[key] = value;
    }

    public static void AddHandle(string key, Object value, GameObject go)
    {
        instances.Add(go);
        Register(key, value);
    }

    public static void AddHandle(string key, Object value, AsyncOperationHandle handle)
    {
        handles.Add(handle);
        Register(key, value);
    }

    public static T Get<T>(string key) where T : Object
    {
        if(assets.TryGetValue(key, out var t) && t is T type)
        {
            return type;
        }
        return null;
    }

    public static void ReleaseAll()
    {
        foreach (var go in instances)
        {
            if (go != null)
                Addressables.ReleaseInstance(go);
        }
        instances.Clear();

        foreach (var handle in handles)
        {
            Addressables.Release(handle);
        }
        handles.Clear();
    }
}
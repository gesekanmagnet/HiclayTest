using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public static class AssetManager
{
    private static List<GameObject> instances = new();
    private static List<AsyncOperationHandle> handles = new();

    public static void AddHandle(GameObject go) => instances.Add(go);
    public static void AddHandle(AsyncOperationHandle handle) => handles.Add(handle);

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
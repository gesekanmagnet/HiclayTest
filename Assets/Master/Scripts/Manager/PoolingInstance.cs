using System.Collections.Generic;

using UnityEngine;

public class PoolingInstance<T> where T : Component
{
    private Queue<T> pool = new();
    private T prefab;
    private Transform parent;

    /// <summary>
    /// Create new instance for pooling system
    /// </summary>
    /// <param name="prefab">Object for instantiate</param>
    /// <param name="count">Count of object to spawn</param>
    /// <param name="parent">Parent for spawned object</param>
    public PoolingInstance(T prefab, int count, Transform parent = null)
    {
        this.prefab = prefab;
        this.parent = parent;

        for (int i = 0; i < count; i++)
        {
            T item = CreateItem();
            item.gameObject.SetActive(false);
            pool.Enqueue(item);
        }
    }

    private T CreateItem()
    {
        T item = Object.Instantiate(prefab, parent);
        item.gameObject.SetActive(true);
        return item;
    }

    /// <summary>
    /// Get the item out from the pool
    /// </summary>
    /// <returns>Object to activate</returns>
    public T GetItem()
    {
        if (pool.Count > 0)
        {
            T item = pool.Dequeue();
            item.gameObject.SetActive(true);
            return item;
        }
        else
        {
            return CreateItem();
        }
    }

    /// <summary>
    /// Return the item back to the pool
    /// </summary>
    /// <param name="item">Object to return</param>
    public void ReturnItem(T item)
    {
        item.gameObject.SetActive(false);
        pool.Enqueue(item);
    }
}
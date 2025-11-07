using System.Collections.Generic;
using UnityEngine;

public static class SimplePool
{
    private class Pool
    {
        public readonly Stack<GameObject> Inactives = new Stack<GameObject>();
        public readonly GameObject Prefab;
        public readonly Transform Root;
        public Pool(GameObject prefab)
        {
            Prefab = prefab;
            Root = new GameObject($"Pool_{prefab.name}").transform;
            Object.DontDestroyOnLoad(Root.gameObject);
        }
    }

    private static readonly Dictionary<int, Pool> pools = new Dictionary<int, Pool>();

    public static GameObject Get(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (prefab == null) return null;
        int key = prefab.GetInstanceID();
        if (!pools.TryGetValue(key, out var pool))
        {
            pool = new Pool(prefab);
            pools.Add(key, pool);
        }

        GameObject go = pool.Inactives.Count > 0 ? pool.Inactives.Pop() : Object.Instantiate(prefab);
        go.transform.SetPositionAndRotation(position, rotation);
        go.transform.SetParent(null, false);
        go.SetActive(true);
        return go;
    }

    public static void Release(GameObject prefab, GameObject instance)
    {
        if (prefab == null || instance == null) return;
        int key = prefab.GetInstanceID();
        if (!pools.TryGetValue(key, out var pool))
        {
            pool = new Pool(prefab);
            pools.Add(key, pool);
        }
        instance.SetActive(false);
        instance.transform.SetParent(pool.Root, false);
        pool.Inactives.Push(instance);
    }
}



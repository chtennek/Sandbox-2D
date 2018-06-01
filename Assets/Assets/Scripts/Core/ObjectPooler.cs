using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPool
{
    public Transform prefab;
    public Transform folder;
    public int initialSize = 3;
    public bool reuseActive = false;
    public bool expandAsNeeded = true;

    private HashSet<Transform> active;
    private Queue<Transform> inactive;
    private Dictionary<Transform, IPooledObject[]> resetLookup;

    public ObjectPool(Transform prefab, Transform folder = null, int initialSize = 3, bool reuseActive = false, bool expandAsNeeded = true)
    {
        this.prefab = prefab;
        this.folder = folder;{}
        this.initialSize = initialSize;
        this.reuseActive = reuseActive;
        this.expandAsNeeded = expandAsNeeded;
        Initialize();
    }

    public void Initialize()
    {
        active = new HashSet<Transform>();
        inactive = new Queue<Transform>();
        resetLookup = new Dictionary<Transform, IPooledObject[]>();

        for (int i = 0; i < initialSize; i++)
        {
            Allocate();
        }
    }

    private Transform Allocate()
    {
        Transform obj = GameObject.Instantiate(prefab);
        ObjectPooler.RegisterPooledObject(obj, this);
        inactive.Enqueue(obj);

        obj.transform.SetParent(folder);
        obj.gameObject.SetActive(false);
        resetLookup.Add(obj, obj.GetComponentsInChildren<IPooledObject>());
        return obj;
    }

    public Transform Instantiate()
    {
        Transform obj;
        if (inactive.Count == 0)
        {
            if (expandAsNeeded == true)
                Allocate();
            else if (reuseActive == true)
            {
                foreach (Transform any in active)
                {
                    Destroy(any);
                    break;
                }
            }
            else
                return null;
        }

        obj = inactive.Dequeue();
        active.Add(obj);

        obj.localPosition = Vector3.zero;
        obj.localRotation = Quaternion.identity;
        obj.localScale = Vector3.one;
        obj.gameObject.SetActive(true);
        foreach (IPooledObject poolable in resetLookup[obj])
            poolable.OnReset();
        return obj.transform;
    }

    public bool Destroy(Transform obj)
    {
        if (active.Contains(obj) == false)
            return false;

        active.Remove(obj);
        inactive.Enqueue(obj);

        obj.transform.SetParent(folder);
        obj.gameObject.SetActive(false);
        return true;
    }
}

public class ObjectPooler : MonoBehaviour
{
    public List<ObjectPool> pools;
    public bool addPoolsAsNeeded = false;
    public bool keepPoolsAsChildren = true;

    private Dictionary<Transform, ObjectPool> poolLookup = new Dictionary<Transform, ObjectPool>(); // Maps prefabs AND their instances to their ObjectPool

    public static ObjectPooler singleton;

    private void Awake()
    {
        if (this.AssertSingleton(ref singleton) == false)
            return;

        foreach (ObjectPool pool in pools)
        {
            poolLookup.Add(pool.prefab, pool);
            if (keepPoolsAsChildren)
                pool.folder = transform;

            pool.Initialize();
        }
    }

    private void AddPool(Transform prefab)
    {
        if (poolLookup.ContainsKey(prefab))
            return;

        ObjectPool pool = new ObjectPool(prefab, keepPoolsAsChildren ? transform : null);
        pools.Add(pool);
        poolLookup.Add(prefab, pool);
    }

    private static bool HasSingleton()
    {
        if (singleton == null)
            Warnings.NoSingleton<ObjectPooler>();
        return singleton != null;
    }

    public static void RegisterPooledObject(Transform obj, ObjectPool pool)
    {
        if (HasSingleton() == false)
            return;

        singleton.poolLookup.Add(obj, pool);
    }

    public static Transform Allocate(Transform prefab, Transform parent = null)
    {
        if (HasSingleton() == false)
            return null;

        if (singleton.addPoolsAsNeeded && singleton.poolLookup.ContainsKey(prefab) == false)
            singleton.AddPool(prefab);

        ObjectPool pool;
        if (singleton.poolLookup.TryGetValue(prefab, out pool) == false)
            return null;

        Transform obj = pool.Instantiate();
        if (obj != null) {
            Vector3 scale = obj.localScale;
            obj.SetParent(parent);
            obj.localScale = scale;
        }
        return obj;
    }

    public static void Deallocate(Transform obj)
    {
        if (HasSingleton() == false)
        {
            Destroy(obj.gameObject);
            return;
        }

        ObjectPool pool;
        singleton.poolLookup.TryGetValue(obj, out pool);
        if (pool != null)
            pool.Destroy(obj);
        else
            Destroy(obj.gameObject);
    }
}

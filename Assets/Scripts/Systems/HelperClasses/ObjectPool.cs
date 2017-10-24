using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    Queue<GameObject> m_pool;
    GameObject m_baseObject;

    Transform poolParent;

    public ObjectPool(GameObject newObject, int Amount = 1)
    {
        m_pool = new Queue<GameObject>();
        m_baseObject = newObject;
        poolParent = new GameObject(m_baseObject.name + "_ObjectPool").transform;
        for (int i = 0; i < Amount; i++)
            AddObject(Object.Instantiate(m_baseObject, poolParent) as GameObject);
    }

    private GameObject FindNextAvailable()
    {
        if (m_pool.Count > 0)
            return RemoveObject();
        return Object.Instantiate(m_baseObject, poolParent) as GameObject;
    }

    private void AddObject(GameObject newObject)
    {
        newObject.SetActive(false);
        m_pool.Enqueue(newObject);
    }

    private GameObject RemoveObject()
    {
        if (m_pool.Peek())
        {
            GameObject newObject = m_pool.Dequeue();
            newObject.SetActive(true);
            return newObject;
        }
        else
        {
            Debug.LogError(poolParent.name + ": Broke, Did you destroy an Item?");
            return null;
        }
    }

    //To get an object out of the pool
    public GameObject NextAvailable
    {
        get { return FindNextAvailable(); }
    }

    // Amount of objects in pool
    public int Capacity
    {
        get { return m_pool.Count; }
    }
}

using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform _canvas; 
    [SerializeField] private int poolSize = 15;
    
    private Queue<GameObject> pool = new Queue<GameObject>();
    private List<GameObject> active = new List<GameObject>();


    private void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab, _canvas);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject Get()
    {
        if (pool.Count == 0)
        {
            Debug.Log("Pool is empty");
            return null;
        }

        GameObject obj = pool.Dequeue();
        active.Add(obj);
        obj.SetActive(true);
        return obj;
    }

    public void Return(GameObject obj)
    {
        if (!active.Contains(obj)) return;
        
        active.Remove(obj);
        obj.SetActive(false);
        pool.Enqueue(obj);
    }

    public bool CanGetObject()
    {
        return pool.Count > 0;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ObjectToPool
{
    public string name;
    public GameObject gameObject;
    public int amount;
    public Transform parent;
}

public class PooledObject
{
    public string name;
    public GameObject gameObject;
    public Transform transform;
    public RectTransform rectTransform;
    public Image image;
}

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;
    public List<ObjectToPool> objectToPool;
    public Dictionary<string, Queue<PooledObject>> poolDictionary;


    private void Awake()
    {
        instance = this;

        poolDictionary = new Dictionary<string, Queue<PooledObject>>();
        foreach (var item in objectToPool)
        {
            var pooledObjectsQ = new Queue<PooledObject>();
            for (var i = 0; i < item.amount; i++)
            {
                var obj = Instantiate(item.gameObject, item.parent);
                obj.SetActive(false);
                pooledObjectsQ.Enqueue(new PooledObject()
                {
                    name = item.name, gameObject = obj, transform = obj.transform,
                    rectTransform = obj.GetComponent<RectTransform>(),
                    image = obj.GetComponent<Image>()
                });
            }

            poolDictionary.Add(item.name, pooledObjectsQ);
        }
    }

    public void ClearPool()
    {
        foreach (var pooledObject in poolDictionary.SelectMany(pair => pair.Value))
        {
            Destroy(pooledObject.gameObject);
        }
    }

    private void Start()
    {
        GameManager.instance.onGameOver += ClearPool;
    }


    public PooledObject GetPooledObject(string objectName)
    {
        if (!poolDictionary.ContainsKey(objectName))
        {
            return null;
        }

        var obj = poolDictionary[objectName].Dequeue();
        poolDictionary[objectName].Enqueue(obj);

        return obj;
    }

    private void OnDestroy()
    {
        GameManager.instance.onGameOver -= ClearPool;
    }
}
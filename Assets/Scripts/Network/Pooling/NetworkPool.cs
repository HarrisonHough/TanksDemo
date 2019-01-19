using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkPool : NetworkBehaviour
{
    [SerializeField]
    private GameObject prefabToPool;
    [SerializeField]
    private int poolSize = 50;

    private Queue<GameObject> objectsQueue = new Queue<GameObject>();
    private List<GameObject> objectPool = new List<GameObject>();

    public NetworkHash128 assetId { get; set; }

    public delegate GameObject SpawnDelegate(Vector3 position, NetworkHash128 assetId);
    public delegate void UnSpawnDelegate(GameObject spawned);

    /// <summary>
    /// Use this for initialization
    /// </summary>
    void Start()
    {
        if (isServer)
            GrowPool();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public GameObject GetObject()
    {
        if (objectsQueue.Count == 0)
        {
            GrowPool();
        }

        var pooledObject = objectsQueue.Dequeue();
        pooledObject.SetActive(true);
        return pooledObject;
    }


    /// <summary>
    /// 
    /// </summary>
    private void GrowPool()
    {
        assetId = prefabToPool.GetComponent<NetworkIdentity>().assetId;

        int lastPoolSize = objectPool.Count;
        for (int i = 0; i < poolSize; i++)
        {

            var pooledObject = Instantiate(prefabToPool);
            pooledObject.name += " " + (i + lastPoolSize);
            pooledObject.transform.parent = transform;
            pooledObject.AddComponent<NetworkPoolMember>();

            //TODO maybe set on disable event
            pooledObject.GetComponent<NetworkPoolMember>().OnDestroyEvent += () => AddObjectToAvailable(pooledObject);


            //add to pool
            objectPool.Add(pooledObject);

            pooledObject.SetActive(false);
        }

        ClientScene.RegisterSpawnHandler(assetId, SpawnObject, UnSpawnObject);
    }

    public GameObject SpawnObject(Vector3 position, NetworkHash128 assetId)
    {
        return GetObject();
    }

    public void UnSpawnObject(GameObject spawned)
    {
        Debug.Log("Re-pooling GameObject " + spawned.name);
        spawned.SetActive(false);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pooledObject"></param>
    private void AddObjectToAvailable(GameObject pooledObject)
    {
        objectsQueue.Enqueue(pooledObject);
    }

    public void DisableAfterDelay(GameObject objectToDisable, float delay)
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkPool : NetworkBehaviour
{
    [SerializeField]
    private GameObject _prefabToPool;
    [SerializeField]
    private int _poolSize = 50;

    private Queue<GameObject> _objectsQueue = new Queue<GameObject>();
    private List<GameObject> _objectPool = new List<GameObject>();

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
        if (_objectsQueue.Count == 0)
        {
            GrowPool();
        }

        var pooledObject = _objectsQueue.Dequeue();
        pooledObject.SetActive(true);
        return pooledObject;
    }


    /// <summary>
    /// 
    /// </summary>
    private void GrowPool()
    {
        assetId = _prefabToPool.GetComponent<NetworkIdentity>().assetId;

        int lastPoolSize = _objectPool.Count;
        for (int i = 0; i < _poolSize; i++)
        {

            var pooledObject = Instantiate(_prefabToPool);
            pooledObject.name += " " + (i + lastPoolSize);
            pooledObject.transform.parent = transform;
            pooledObject.AddComponent<NetworkPoolMember>();

            //TODO maybe set on disable event
            pooledObject.GetComponent<NetworkPoolMember>().OnDestroyEvent += () => AddObjectToAvailable(pooledObject);


            //add to pool
            _objectPool.Add(pooledObject);

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
        _objectsQueue.Enqueue(pooledObject);
    }

    public void DisableAfterDelay(GameObject objectToDisable, float delay)
    {

    }
}

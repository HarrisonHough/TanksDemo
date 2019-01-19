using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkFXController : NetworkBehaviour
{

    [SerializeField]
    private Pool hitFXPool;
    [SerializeField]
    private Pool spawnFXPool;
    [SerializeField]
    private Pool explodeFXPool;

    public void SpawnHitFX(Vector3 position)
    {
        GameObject FX = hitFXPool.GetObject();
        FX.transform.position = position;
        FX.SetActive(true);
    }
    public void SpawnFX(Vector3 position)
    {
        GameObject FX = spawnFXPool.GetObject();
        FX.transform.position = position;
        FX.SetActive(true);
    }
    public void SpawnExplodeFX(Vector3 position)
    {
        GameObject FX = explodeFXPool.GetObject();
        FX.transform.position = position;
        FX.SetActive(true);
    }
}

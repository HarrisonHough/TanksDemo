using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkFXController : NetworkBehaviour
{

    [SerializeField]
    private Pool _hitFXPool;
    [SerializeField]
    private Pool _spawnFXPool;
    [SerializeField]
    private Pool _explodeFXPool;

    public void SpawnHitFX(Vector3 position)
    {
        GameObject FX = _hitFXPool.GetObject();
        FX.transform.position = position;
        FX.SetActive(true);
    }
    public void SpawnFX(Vector3 position)
    {
        GameObject FX = _spawnFXPool.GetObject();
        FX.transform.position = position;
        FX.SetActive(true);
    }
    public void SpawnExplodeFX(Vector3 position)
    {
        GameObject FX = _explodeFXPool.GetObject();
        FX.transform.position = position;
        FX.SetActive(true);
    }
}

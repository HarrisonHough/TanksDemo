using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkPlayerShoot : NetworkBehaviour
{
    [SerializeField]
    private GameObject _bulletPrefab;
    [SerializeField]
    private Transform _bulletSpawnPoint;
    [SerializeField]
    private int _shotsPerBurst = 2;

    private int _shotsLeft = 0;
    private bool _isReloading = false;
    [SerializeField]
    private float _reloadTime = 1f;

    public LayerMask obstacleMask;
    private bool _canShoot = false;

    private void OnEnable()
    {
        Reset();
    }

    private void OnDisable()
    {
        _canShoot = false;
    }

    public void Enable()
    {
        _canShoot = true;
    }

    public void Disable()
    {
        _canShoot = false;
    }

    public void Shoot()
    {
        if (_isReloading || !_canShoot)
        {
            return;
        }

        RaycastHit hit;
        Vector3 center = new Vector3(transform.position.x, _bulletSpawnPoint.position.y, transform.position.z);
        Vector3 direction = (_bulletSpawnPoint.position - center).normalized;

        if (Physics.SphereCast(center, 0.25f, direction, out hit, 2.6f, obstacleMask, QueryTriggerInteraction.Ignore))
        {
            //Miss Fire
        }
        else
        {
            //SpawnBullet();
            CmdSpawnBullet();
            _shotsLeft--;
            if (_shotsLeft <= 0)
            {
                StartCoroutine(Reload());
            }
        }
    }

    [Command]
    private void CmdSpawnBullet()
    {
       

        GameObject bullet = NetworkGameManager.Instance.BulletPool.GetObject();
        bullet.transform.position = _bulletSpawnPoint.position;
        bullet.transform.rotation = _bulletSpawnPoint.rotation;
        //bullet.GetComponent<NetworkBullet>().SetVelocity(bulletSpawnPoint);
        bullet.GetComponent<NetworkBullet>().Launch(_bulletSpawnPoint, this.GetComponent<NetworkPlayerController>());

        NetworkServer.Spawn(bullet, NetworkGameManager.Instance.BulletPool.assetId);
    }

    IEnumerator Reload()
    {
        _shotsLeft = _shotsPerBurst;
        _isReloading = true;
        float timeElapsed = 0;
        while (timeElapsed < _reloadTime)
        {
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        _isReloading = false;
    }

    public void Reset()
    {
        _shotsLeft = _shotsPerBurst;
        _isReloading = false;
        _canShoot = true;
    }
}

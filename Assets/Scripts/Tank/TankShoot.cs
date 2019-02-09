using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShoot : MonoBehaviour
{
    [SerializeField]
    private Transform _bulletSpawnPoint;
    [SerializeField]
    private int _shotsPerBurst = 2;

    private int _shotsLeft = 0;
    private bool _isReloading = false;
    [SerializeField]
    private float _reloadTime = 1f;

    public LayerMask ObstacleMask;
    private bool _canShoot = false;

    private void OnEnable()
    {
        _shotsLeft = _shotsPerBurst;
        _isReloading = false;
        _canShoot = true;
    }

    private void OnDisable()
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

        if (Physics.SphereCast(center, 0.25f, direction, out hit, 2.6f, ObstacleMask, QueryTriggerInteraction.Ignore))
        {
            //Miss Fire
        }
        else
        {
            SpawnBullet();

            if (_shotsLeft <= 0)
            {
                StartCoroutine(Reload());
            }
        }
    }

    private void SpawnBullet()
    {
        GameObject bullet = GameManager.Instance.BulletPool.GetObject();
        bullet.transform.position = _bulletSpawnPoint.position;
        bullet.transform.rotation = _bulletSpawnPoint.rotation;
        bullet.SetActive(true);
        bullet.GetComponent<Bullet>().SetVelocity(_bulletSpawnPoint);
        _shotsLeft--;
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShoot : MonoBehaviour
{
    [SerializeField]
    private Transform bulletSpawnPoint;
    [SerializeField]
    private int shotsPerBurst = 2;

    private int shotsLeft = 0;
    bool isReloading = false;
    [SerializeField]
    private float reloadTime = 1f;

    public LayerMask obstacleMask;
    bool canShoot = false;

    private void OnEnable()
    {
        shotsLeft = shotsPerBurst;
        isReloading = false;
        canShoot = true;
    }

    private void OnDisable()
    {
        canShoot = false;
    }

    public void Shoot()
    {
        if (isReloading || !canShoot)
        {
            return;
        }

        RaycastHit hit;
        Vector3 center = new Vector3(transform.position.x, bulletSpawnPoint.position.y, transform.position.z);
        Vector3 direction = (bulletSpawnPoint.position - center).normalized;

        if (Physics.SphereCast(center, 0.25f, direction, out hit, 2.6f, obstacleMask, QueryTriggerInteraction.Ignore))
        {
            //Miss Fire
        }
        else
        {
            SpawnBullet();

            if (shotsLeft <= 0)
            {
                StartCoroutine(Reload());
            }
        }
    }

    private void SpawnBullet()
    {
        GameObject bullet = GameManager.Instance.BulletPool.GetObject();
        bullet.transform.position = bulletSpawnPoint.position;
        bullet.transform.rotation = bulletSpawnPoint.rotation;
        bullet.SetActive(true);
        bullet.GetComponent<Bullet>().SetVelocity(bulletSpawnPoint);
        shotsLeft--;
    }

    IEnumerator Reload()
    {
        shotsLeft = shotsPerBurst;
        isReloading = true;
        float timeElapsed = 0;
        while (timeElapsed < reloadTime)
        {
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        isReloading = false;
    }
}

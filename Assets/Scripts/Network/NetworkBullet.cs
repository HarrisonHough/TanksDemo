using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkBullet : NetworkBehaviour
{
    private Rigidbody rigidbody;
    [SerializeField]
    private Collider collider;
    [SerializeField]
    private int maxBounces = 3;
    private int bounces = 0;

    [SerializeField]
    private int damage = 1;
    [SerializeField]
    private float speed = 100;
    public float Speed { get { return speed; } }



    // Start is called before the first frame update
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        rigidbody.velocity = speed * transform.forward;
    }

    public void SetVelocity(Transform spawnPoint)
    {
        rigidbody.velocity = speed * spawnPoint.transform.forward;
    }

    private void OnDisable()
    {
        rigidbody.velocity = Vector3.zero;
    }

    private void Explode()
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.Sleep();
        NetworkGameManager.Instance.SpawnHitFX(transform.position);
// 
//         GameObject hitFX = NetworkGameManager.Instance.HitFXPool.GetObject();
//         hitFX.transform.position = transform.position;
//         hitFX.SetActive(true);

        //disable self

        NetworkGameManager.Instance.BulletPool.UnSpawnObject(gameObject);
        NetworkServer.UnSpawn(gameObject);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (rigidbody.velocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(rigidbody.velocity);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        CheckCollisions(collision);

        if (bounces > +maxBounces)
        {
            Explode();
        }

        bounces++;
    }

    private void CheckCollisions(Collision collision)
    {

        if (collision.gameObject.tag.Contains("Player") || collision.gameObject.tag.Contains("Enemy"))
        {

            Explode();
            IDamagable<int> damagableObject = collision.gameObject.GetComponent<IDamagable<int>>();
            if (damagableObject != null)
            {
                damagableObject.Damage(damage, gameObject);
            }
        }
    }
}

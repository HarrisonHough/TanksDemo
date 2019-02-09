using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkBullet : NetworkBehaviour
{
    private Rigidbody _rigidbody;
    [SerializeField]
    private Collider _collider;
    [SerializeField]
    private int _maxBounces = 3;
    private int _bounces = 0;

    [SerializeField]
    private int _damage = 1;
    [SerializeField]
    private float _speed = 100;
    public float Speed { get { return _speed; } }

    private NetworkPlayerController _owner;

    // Start is called before the first frame update
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        _rigidbody.velocity = _speed * transform.forward;
    }

    public void SetVelocity(Transform spawnPoint)
    {
        _rigidbody.velocity = _speed * spawnPoint.transform.forward;
    }
    public void Launch(Transform spawnPoint, NetworkPlayerController owner)
    {
        this._owner = owner;
        _rigidbody.velocity = _speed * spawnPoint.transform.forward;
    }

    private void OnDisable()
    {
        _rigidbody.velocity = Vector3.zero;
        _bounces = 0;
    }

    private void Explode()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.Sleep();
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
        if (_rigidbody.velocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(_rigidbody.velocity);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        CheckCollisions(collision);

        if (_bounces > +_maxBounces)
        {
            Explode();
        }

        _bounces++;
    }

    private void CheckCollisions(Collision collision)
    {

        if (collision.gameObject.tag.Contains("Player") || collision.gameObject.tag.Contains("Enemy"))
        {

            Explode();
            NetworkPlayerHealth playerHit = collision.gameObject.GetComponent<NetworkPlayerHealth>();
            if (playerHit != null)
            {
                playerHit.Damage(_damage, _owner);
            }

//             IDamagable<int> damagableObject = collision.gameObject.GetComponent<IDamagable<int>>();
//             if (damagableObject != null)
//             {
//                 damagableObject.Damage(damage, gameObject);
//             }
        }
    }
}

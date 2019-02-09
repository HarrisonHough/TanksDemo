using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
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



    // Start is called before the first frame update
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    public void SetVelocity(Transform spawnPoint)
    {
        _rigidbody.velocity = _speed * spawnPoint.transform.forward;
    }

    private void OnDisable()
    {
        _rigidbody.velocity = Vector3.zero;
    }
    private void Explode()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.Sleep();

        GameObject hitFX = GameManager.Instance.HitFXPool.GetObject();
        hitFX.transform.position = transform.position;
        hitFX.SetActive(true);

        //disable self
        gameObject.SetActive(false);
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
            IDamagable<int> damagableObject = collision.gameObject.GetComponent<IDamagable<int>>();
            if (damagableObject != null)
            {
                damagableObject.Damage(_damage, gameObject);
            }
        }
    }
}

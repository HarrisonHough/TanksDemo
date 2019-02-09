using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Rigidbody))]
public class NetworkPlayerMotor : NetworkBehaviour
{


    [SerializeField]
    private Transform _baseTransform;
    public Transform BaseTransform { get { return _baseTransform; } }
    [SerializeField]
    private Transform _turretTransform;
    public Transform TurretTransform { get { return _turretTransform; } }
    [SerializeField]
    private float _moveSpeed = 100f;
    [SerializeField]
    private float _baseRotateSpeed = 1f;
    [SerializeField]
    private float _turretRotateSpeed = 3f;

    private Rigidbody _rigidbody;

    private bool _canMove = false;

    // Use this for initialization
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

    }

    public void Enable()
    {
        _canMove = true;
    }
    public void Disable()
    {
        _canMove = false;
    }

    public void MovePlayer(Vector3 moveVector)
    {
        if (_canMove) {
            _rigidbody.velocity = moveVector * _moveSpeed;
        }
    }

    public void FaceDirection(Transform xform, Vector3 dir, float rotSpeed)
    {
        if (dir != Vector3.zero && xform != null)
        {
            Quaternion desiredRot = Quaternion.LookRotation(dir);
            xform.rotation = Quaternion.Slerp(xform.rotation, desiredRot, rotSpeed * Time.deltaTime);

        }

    }

    public void RotateChassis(Vector3 direction)
    {
        FaceDirection(_baseTransform, direction, _baseRotateSpeed);
    }


    public void RotateTurret(Vector3 direction)
    {
        FaceDirection(_turretTransform, direction, _turretRotateSpeed);
    }

    public void StopMovement()
    {
        _rigidbody.velocity = Vector3.zero;
        _canMove = false;
    }

    public void Reset()
    {
        StopMovement();
        _canMove = true;
    }
}

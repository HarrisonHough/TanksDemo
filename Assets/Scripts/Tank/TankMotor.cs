using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMotor : MonoBehaviour
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
    [SerializeField]
    private Rigidbody _rigidbody;

    // Use this for initialization
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void MoveTank(Vector3 direction)
    {
        if (GameManager.Instance.Gamestate != GameState.InGame)
        {
            _rigidbody.velocity = Vector3.zero;
            return;
        }
        Vector3 moveDirection = direction * _moveSpeed * Time.deltaTime;

        _rigidbody.velocity = moveDirection;
        //Not using
        //rigidbody.MovePosition(rigidbody.position + direction * Time.deltaTime);
    }

    public void FaceDirection(Transform xform, Vector3 dir, float rotSpeed)
    {
        if (dir != Vector3.zero && xform != null)
        {
            Quaternion desiredRot = Quaternion.LookRotation(dir);
            xform.rotation = Quaternion.Slerp(xform.rotation, desiredRot, rotSpeed * Time.deltaTime);

        }

    }

    public void RotateChassis(Vector3 dir)
    {
        FaceDirection(_baseTransform, dir, _baseRotateSpeed);
    }


    public void RotateTurret(Vector3 dir)
    {
        FaceDirection(_turretTransform, dir, _turretRotateSpeed);
    }
}

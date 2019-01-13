using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMotor : MonoBehaviour
{

    [SerializeField]
    private Transform baseTransform;
    public Transform BaseTransform { get { return baseTransform; } }
    [SerializeField]
    private Transform turretTransform;
    public Transform TurretTransform { get { return turretTransform; } }
    [SerializeField]
    private float moveSpeed = 100f;
    [SerializeField]
    private float baseRotateSpeed = 1f;
    [SerializeField]
    private float turretRotateSpeed = 3f;
    [SerializeField]
    private Rigidbody rigidbody;

    // Use this for initialization
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    public void MoveTank(Vector3 direction)
    {
        if (GameManager.Instance.Gamestate != GameState.InGame)
        {
            rigidbody.velocity = Vector3.zero;
            return;
        }
        Vector3 moveDirection = direction * moveSpeed * Time.deltaTime;

        rigidbody.velocity = moveDirection;
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
        FaceDirection(baseTransform, dir, baseRotateSpeed);
    }


    public void RotateTurret(Vector3 dir)
    {
        FaceDirection(turretTransform, dir, turretRotateSpeed);
    }
}

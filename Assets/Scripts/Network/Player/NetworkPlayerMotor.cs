﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Rigidbody))]
public class NetworkPlayerMotor : NetworkBehaviour
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

    private Rigidbody rigidbody;

    private bool canMove = false;

    // Use this for initialization
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();

    }

    public void Enable()
    {
        canMove = true;
    }
    public void Disable()
    {
        canMove = false;
    }

    public void MovePlayer(Vector3 moveVector)
    {
        if (canMove) {
            rigidbody.velocity = moveVector * moveSpeed;
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
        FaceDirection(baseTransform, direction, baseRotateSpeed);
    }


    public void RotateTurret(Vector3 direction)
    {
        FaceDirection(turretTransform, direction, turretRotateSpeed);
    }

    public void StopMovement()
    {
        rigidbody.velocity = Vector3.zero;
        canMove = false;
    }

    public void Reset()
    {
        StopMovement();
        canMove = true;
    }
}

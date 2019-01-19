using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BaseController : MonoBehaviour
{
    protected TankHealth health;
    protected TankMotor motor;
    protected TankSetup setup;
    protected TankShoot shoot;

    protected Vector3 inputVector;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        health = GetComponent<TankHealth>();
        motor = GetComponent<TankMotor>();
        setup = GetComponent<TankSetup>();
        shoot = GetComponent<TankShoot>();
        
    }

    protected virtual void Update()
    {
        if (GameManager.Instance.Gamestate != GameState.InGame)
            return;

        inputVector = GetInputVector();
        UpdateChassisRotation();
        UpdateTurretRotation();
        ShootCheck();
    }

    protected virtual void FixedUpdate()
    {
        if (GameManager.Instance.Gamestate != GameState.InGame)
            return;

        motor.MoveTank(inputVector);
    }

    protected virtual void UpdateTurretRotation()
    {
        Vector3 turretDir = Utility.GetWorldPointFromScreenPoint(Input.mousePosition, motor.TurretTransform.position.y) - motor.TurretTransform.position;
        motor.RotateTurret(turretDir);
    }

    protected virtual void UpdateChassisRotation()
    {
        
        if (inputVector.sqrMagnitude > 0.25f)
        {
            motor.RotateChassis(inputVector);
        }
    }

    protected virtual void Move()
    {
        motor.MoveTank(inputVector);
    }

 

    protected virtual Vector3 GetInputVector()
    {
        return new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    }

    protected virtual void ShootCheck()
    {
        if(GameManager.Instance.Gamestate == GameState.InGame)
            if (Input.GetMouseButtonDown(0))
            {
                shoot.Shoot();
            }
    }
}

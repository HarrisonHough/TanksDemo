using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TankController : BaseController
{
    [SerializeField]
    private FixedJoystick moveJoystick;
    [SerializeField]
    private FixedJoystick aimJoystick;

    //private bool shoot = false;

    private Vector2 lastAimJoystickVector = Vector2.zero;

    protected override void Start()
    {
        base.Start();

    }

    //only use touch joystick on android
#if UNITY_ANDROID
    protected override void UpdateTurretRotation()
    {
        motor.RotateTurret(new Vector3(aimJoystick.Horizontal, 0, aimJoystick.Vertical));
    }

    protected override Vector3 GetInputVector()
    {
    return new Vector3(moveJoystick.Horizontal, 0, moveJoystick.Vertical).normalized;
    }

    protected override void ShootCheck()
    {
        if (aimJoystick.Direction == Vector2.zero && lastAimJoystickVector != Vector2.zero)
        {
            //released (shoot
            shoot.Shoot();
        }
        lastAimJoystickVector = aimJoystick.Direction;
    }
#endif

}

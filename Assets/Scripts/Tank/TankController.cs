using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TankController : BaseController
{
    [SerializeField]
    private FixedJoystick _moveJoystick;
    [SerializeField]
    private FixedJoystick _aimJoystick;

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
        motor.RotateTurret(new Vector3(_aimJoystick.Horizontal, 0, _aimJoystick.Vertical));
    }

    protected override Vector3 GetInputVector()
    {
    return new Vector3(_moveJoystick.Horizontal, 0, _moveJoystick.Vertical).normalized;
    }

    protected override void ShootCheck()
    {
        if (_aimJoystick.Direction == Vector2.zero && lastAimJoystickVector != Vector2.zero)
        {
            //released (shoot
            shoot.Shoot();
        }
        lastAimJoystickVector = _aimJoystick.Direction;
    }
#endif

}

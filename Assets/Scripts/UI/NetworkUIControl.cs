using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkUIControl : MonoBehaviour
{

    [SerializeField]
    private GameObject joysticks;
    [SerializeField]
    private FixedJoystick moveJoystick;
    public FixedJoystick MoveJoystick { get { return moveJoystick; } }
    [SerializeField]
    private FixedJoystick aimJoystick;
    public FixedJoystick AimJoystick { get { return aimJoystick; } }

    // Use this for initialization
    void Start()
    {

        //only use touch joystick on android
#if UNITY_ANDROID
        joysticks.SetActive(true);
#else
        joysticks.SetActive(false);
#endif

    }
}

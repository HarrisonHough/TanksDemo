using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

public class NetworkPlayerController : NetworkBehaviour
{

    [SerializeField]
    private FixedJoystick _moveJoystick;
    [SerializeField]
    private FixedJoystick _aimJoystick;
    private Vector2 _lastAimJoystickVector = Vector2.zero;

    protected NetworkPlayerHealth health;
    protected NetworkPlayerMotor motor;
    protected NetworkPlayerSetup setup;
    protected NetworkPlayerShoot shoot;

    protected Vector3 inputVector;

    [SerializeField]
    private Behaviour[] _nonLocalcomponentsToDisable;

    public UnityEvent OnDeathEvent;
    public UnityEvent OnRespawnEvent;

    private bool _isDead = false;
    [SyncVar]
    public int Score = 0;

    private float _respawnTime = 5f;


    // Start is called before the first frame update
    private void Start()
    {
        health = GetComponent<NetworkPlayerHealth>();
        motor = GetComponent<NetworkPlayerMotor>();
        setup = GetComponent<NetworkPlayerSetup>();
        shoot = GetComponent<NetworkPlayerShoot>();

        _moveJoystick = NetworkGameManager.Instance.UIControl.MoveJoystick;
        _aimJoystick = NetworkGameManager.Instance.UIControl.AimJoystick;

        if (!isLocalPlayer)
        {
            DisableNonLocalPlayerComponents();
        }
    }

    private void Update()
    {
        if (!isLocalPlayer || _isDead)
            return;

        inputVector = GetInputVector();
        //Debug.Log("Input Vector is " + inputVector);
        UpdateChassisRotation();
        UpdateTurretRotation();
        ShootCheck();
    }

    private void FixedUpdate()
    {
        if (NetworkGameManager.Instance.Gamestate != GameState.InGame || !isLocalPlayer || _isDead)
            return;

        motor.MovePlayer(inputVector * Time.deltaTime);
    }

    //only use touch joystick on android
#if UNITY_ANDROID
    private void UpdateTurretRotation()
    {
        motor.RotateTurret(new Vector3(_aimJoystick.Horizontal, 0, _aimJoystick.Vertical));
    }

    private Vector3 GetInputVector()
    {
        return new Vector3(_moveJoystick.Horizontal, 0, _moveJoystick.Vertical).normalized;
    }

    private void ShootCheck()
    {
        if (_aimJoystick.Direction == Vector2.zero && _lastAimJoystickVector != Vector2.zero)
        {
            //released (shoot
            shoot.Shoot();
        }
        _lastAimJoystickVector = _aimJoystick.Direction;
    }

#endif

    private void UpdateChassisRotation()
    {

        if (inputVector.sqrMagnitude > 0.25f)
        {
            motor.RotateChassis(inputVector);
        }
    }

#if UNITY_STANDALONE_WIN

    private void UpdateTurretRotation()
    {
        Vector3 turretDirection = Utility.GetWorldPointFromScreenPoint(Input.mousePosition, motor.TurretTransform.position.y) - motor.TurretTransform.position;

        //Debug.Log("turret Direction" + turretDirection + "\n Mouse Position = " + Input.mousePosition);
        motor.RotateTurret(turretDirection);
    }

   
     private Vector3 GetInputVector()
    {
        return new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    }

    private void ShootCheck()
    {
        if (GameManager.Instance.Gamestate == GameState.InGame)
            if (Input.GetMouseButtonDown(0))
            {
                shoot.Shoot();
            }
    }


#endif
    private void Move()
    {
        motor.MovePlayer(inputVector);
    }
   
    public void Reset()
    {
        if (_moveJoystick == null)
        {
            _moveJoystick = NetworkGameManager.Instance.UIControl.MoveJoystick;
           
        }
        if (_aimJoystick == null)
        {
           
            _aimJoystick = NetworkGameManager.Instance.UIControl.AimJoystick;
        }
        health.ResetPlayerHealth();
        motor.Reset();
        shoot.Reset();

    }

    private void PlayerRespawn()
    {
        //spawn at new position
        //TODO create spawn point system

        Transform newSpawnPoint = NetworkGameManager.Instance.GetSpawnPoint();
        transform.position = newSpawnPoint.position;
        transform.rotation = newSpawnPoint.rotation;
        
        OnRespawnEvent.Invoke();
        Reset();
        _isDead = false;
        NetworkGameManager.Instance.SpawnFX(transform.position);

    }

    public void Death()
    {
        if (!_isDead)
        {
            StartCoroutine(RespawnRoutine());
        }

    }


    private void DisableNonLocalPlayerComponents()
    {
        //hide components
        foreach (Behaviour component in _nonLocalcomponentsToDisable)
        {
            component.enabled = false;
        }
    }

    IEnumerator RespawnRoutine()
    {
        motor.Reset();
        _isDead = true;
        OnDeathEvent.Invoke();
        float timeToRespawn = Time.time + _respawnTime;
        while (Time.time < timeToRespawn)
        {
            yield return null;
        }

        //wait for time
        PlayerRespawn();
        
    }

    public void EnablePlayerControls()
    {
        motor.Enable();
        shoot.Enable();
    }

    public void DisablePlayerControls()
    {
        motor.Disable();
        shoot.Disable();
    }
}

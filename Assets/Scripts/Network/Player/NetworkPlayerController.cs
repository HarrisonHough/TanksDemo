using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

public class NetworkPlayerController : NetworkBehaviour
{

    [SerializeField]
    private FixedJoystick moveJoystick;
    [SerializeField]
    private FixedJoystick aimJoystick;

    protected NetworkPlayerHealth health;
    protected NetworkPlayerMotor motor;
    protected NetworkPlayerSetup setup;
    protected NetworkPlayerShoot shoot;

    protected Vector3 inputVector;

    [SerializeField]
    private Behaviour[] nonLocalcomponentsToDisable;

    public UnityEvent OnDeathEvent;
    public UnityEvent OnRespawnEvent;

    private bool isDead = false;
    [SyncVar]
    public int Score = 0;

    private float respawnTime = 5f;
    // Start is called before the first frame update
    private void Start()
    {
        health = GetComponent<NetworkPlayerHealth>();
        motor = GetComponent<NetworkPlayerMotor>();
        setup = GetComponent<NetworkPlayerSetup>();
        shoot = GetComponent<NetworkPlayerShoot>();

        if (!isLocalPlayer)
        {
            DisableNonLocalPlayerComponents();
        }
    }

    private void Update()
    {
        if (!isLocalPlayer || isDead)
            return;

        inputVector = GetInputVector();
        //Debug.Log("Input Vector is " + inputVector);
        UpdateChassisRotation();
        UpdateTurretRotation();
        ShootCheck();
    }

    private void FixedUpdate()
    {
        if (NetworkGameManager.Instance.Gamestate != GameState.InGame || !isLocalPlayer || isDead)
            return;

        motor.MovePlayer(inputVector * Time.deltaTime);
    }

    private void UpdateTurretRotation()
    {
        Vector3 turretDirection = Utility.GetWorldPointFromScreenPoint(Input.mousePosition, motor.TurretTransform.position.y) - motor.TurretTransform.position;

        //Debug.Log("turret Direction" + turretDirection + "\n Mouse Position = " + Input.mousePosition);
        motor.RotateTurret(turretDirection);
    }

    private void UpdateChassisRotation()
    {

        if (inputVector.sqrMagnitude > 0.25f)
        {
            motor.RotateChassis(inputVector);
        }
    }

    private void Move()
    {
        motor.MovePlayer(inputVector);
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

    public void Reset()
    {
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
        isDead = false;
        NetworkGameManager.Instance.SpawnFX(transform.position);

    }

    public void Death()
    {
        if (!isDead)
        {
            StartCoroutine(RespawnRoutine());
        }

    }


    private void DisableNonLocalPlayerComponents()
    {
        //hide components
        foreach (Behaviour component in nonLocalcomponentsToDisable)
        {
            component.enabled = false;
        }
    }

    IEnumerator RespawnRoutine()
    {
        isDead = true;
        OnDeathEvent.Invoke();
        float timeToRespawn = Time.time + respawnTime;
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

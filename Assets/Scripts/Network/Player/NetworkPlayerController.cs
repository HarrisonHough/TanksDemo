using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class NetworkPlayerController : NetworkBehaviour {

    private NetworkPlayerHealth playerHealth;
    private NetworkPlayerMotor playerMotor;
    private NetworkPlayerSetup playerSetup;
    private NetworkPlayerShoot playerShoot;

    // Use this for initialization
    void Start() {
        playerHealth = GetComponent<NetworkPlayerHealth>();
        playerMotor = GetComponent<NetworkPlayerMotor>();
        playerSetup = GetComponent<NetworkPlayerSetup>();
        playerShoot = GetComponent<NetworkPlayerShoot>();

    }

    Vector3 GetInput() {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        return new Vector3(h, 0, v);
    }

    private void FixedUpdate()
    {
        //if not local player
        if (!isLocalPlayer)
            return;

        Vector3 inputDirection = GetInput();
        playerMotor.MovePlayer(inputDirection);
    }

    private void Update()
    {
        //if not local player
        if (!isLocalPlayer)
            return;

        Vector3 inputDirection = GetInput();
        if (inputDirection.sqrMagnitude > 0.25f)
        {
            playerMotor.RotateChassis(inputDirection);
        }

        Vector3 turretDir = Utility.GetWorldPointFromScreen(Input.mousePosition, playerMotor.TurretTransform.position.y) - playerMotor.TurretTransform.position;
        playerMotor.RotateTurret(turretDir);
    }
}

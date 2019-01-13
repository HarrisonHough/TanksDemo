using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AITankController : BaseController
{
    [SerializeField]
    private NavMeshAgent agent;

    [SerializeField]
    private float attackRange = 20f;
    [SerializeField]
    private float attackInterval = 0.5f;

    private float nextTimeCanAttack = 0;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
    }

    protected override void Update()
    {
        if (GameManager.Instance == null)
        { return; }

        if (GameManager.Instance.Gamestate != GameState.InGame)
        {
            agent.isStopped = true;
            return;
        }
        else
            agent.isStopped = false;


        agent.SetDestination(GameManager.Instance.Player.transform.position);
        UpdateChassisRotation();
        UpdateTurretRotation();
        ShootCheck();
    }

    protected override void FixedUpdate()
    { 
        motor.MoveTank(agent.velocity);
    }

    protected override void UpdateTurretRotation()
    {
        Vector3 turretDir = GameManager.Instance.Player.transform.position - motor.TurretTransform.position;
        motor.RotateTurret(turretDir);
    }

    protected override void UpdateChassisRotation()
    {
        if (agent.velocity.sqrMagnitude > 0.25f)
        {
            motor.RotateChassis(agent.velocity);
        }
    }

    protected override void ShootCheck()
    {
        if (GameManager.Instance.Gamestate != GameState.InGame) { return; }
        //Debug.Log("Distance = " + (transform.position - GameManager.Instance.Player.transform.position).sqrMagnitude / 10 + "\n range = " + attackRange);
        if ((transform.position - GameManager.Instance.Player.transform.position).sqrMagnitude / 10 < attackRange)
        {
            //Debug.Log("AI is in Shooting Distance");
            if (Time.time > nextTimeCanAttack)
            {
                //Debug.Log("AI Shooting");
                shoot.Shoot();
                nextTimeCanAttack = Time.time + attackInterval;

            }
        }
        
    }

    
}

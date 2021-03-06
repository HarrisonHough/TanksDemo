﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class NetworkPlayerHealth : NetworkBehaviour
{
    [SyncVar(hook = "UpdateHealthBar")]
    private int _currentHealth;
    [SerializeField]
    private int _maxHealth = 3;
    public int MaxHealth { get { return _maxHealth; } }

    [SyncVar]
    public bool isDead = false;
    //ToDo check if references work
//     public bool IsDead
//     {
//         get { return isDead; }
//     }

    private NetworkPlayerController lastAttacker;
    private float lastDamageTime = 0;
    [SerializeField]
    private Slider healthSlider;
    [SerializeField]
    private NetworkPlayerController playerController;

    private void Start()
    {
        playerController = GetComponent<NetworkPlayerController>();
        ResetPlayerHealth();
    }

    public void ResetPlayerHealth()
    {
        _currentHealth = _maxHealth;
        healthSlider.maxValue = _currentHealth;
        UpdateHealthBar((int)healthSlider.maxValue);

        isDead = false;
    }

    public void Damage(int damage, NetworkPlayerController attacker)
    {
        if (!isServer)
        {
            return;
        }

        if (Time.time - lastDamageTime < 0.1f)
            return;
        lastDamageTime = Time.time;
        _currentHealth -= damage;
        Debug.Log("Damage Taken By AI");
        if(attacker != playerController)
            lastAttacker = attacker;
        //TODO fix update health bar
        UpdateHealthBar(_currentHealth);

        if (_currentHealth <= 0 && !isDead)
        {
            //add to attackers score
            if (lastAttacker != null)
            {
                lastAttacker.Score++;
            }
            //or do nothing if you killed yourself-or subtract from score?
            NetworkGameManager.Instance.UpdateScoreboard();

            isDead = true;
            RpcDie();

            //TODO Check this works
            playerController.Death();
        }
    }

    private void UpdateHealthBar(int health)
    {
        healthSlider.value = health;
    }

    [ClientRpc]
    private void RpcDie()
    {
        ShowDeathParticles();

        
        Debug.Log(gameObject.name + " Is Now Dead");

        //start respawn process

        //TODO create disable function
        playerController.Death();
    }

    private void ShowDeathParticles()
    {

        NetworkGameManager.Instance.SpawnExplodeFX(transform.position);
    }
}

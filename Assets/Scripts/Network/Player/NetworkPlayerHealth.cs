using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class NetworkPlayerHealth : NetworkBehaviour, IDamagable<int>
{
    [SyncVar(hook = "UpdateHealthBar")]
    private int currentHealth;
    [SerializeField]
    private int maxHealth = 3;
    public int MaxHealth { get { return maxHealth; } }

    [SyncVar]
    public bool isDead = false;
    //ToDo check if references work
//     public bool IsDead
//     {
//         get { return isDead; }
//     }

    private GameObject lastAttacker;
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
        currentHealth = maxHealth;
        healthSlider.maxValue = currentHealth;
        UpdateHealthBar((int)healthSlider.maxValue);

        isDead = false;
    }

    public void Damage(int damage, GameObject enemyObject)
    {
        if (!isServer)
        {
            return;
        }

        if (Time.time - lastDamageTime < 0.1f)
            return;
        lastDamageTime = Time.time;
        currentHealth -= damage;
        Debug.Log("Damage Taken By AI");
        lastAttacker = enemyObject;
        //TODO fix update health bar
        UpdateHealthBar(currentHealth);

        if (currentHealth <= 0 && !isDead)
        {
            //GameManager.Instance.UpdateScoreboard();
            isDead = true;
            RpcDie();
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
        //TODO upgrade to Network gameManager
//         GameObject explodeFX = NetworkGameManager.Instance.ExplodeFXPool.GetObject();
//         explodeFX.transform.position = transform.position;
//         explodeFX.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour, IDamagable<int>
{
    private int currentHealth;
    [SerializeField]
    private int maxHealth = 3;
    public int MaxHealth { get { return maxHealth; } }

    private bool isDead = false;

    private GameObject lastAttacker;
    private float lastDamageTime = 0;
    [SerializeField]
    private Slider healthSlider;

    void OnEnable()
    {
        ResetPlayerHealth();
    }

    void ResetPlayerHealth()
    {
        currentHealth = maxHealth;
        healthSlider.maxValue = currentHealth;
        UpdateHealthBar((int)healthSlider.maxValue);

        isDead = false;        
    }

    public void Damage(int damage, GameObject enemyObject)
    {
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
            Die();
        }
    }

    private void UpdateHealthBar(int health)
    {
        healthSlider.value = health;
    }

    private void Die()
    {
        GameObject explodeFX =  GameManager.Instance.ExplodeFXPool.GetObject();
        explodeFX.transform.position = transform.position;
        explodeFX.SetActive(true);
        isDead = true;
        Debug.Log(gameObject.name + " Is Now Dead");

        //check if player
        if (gameObject.tag.Contains("Player"))
        {
            GameManager.Instance.GameOver();
        }
        else
        {
            GameManager.Instance.TanksDestroyed++;
            GameManager.Instance.ActiveAITanks.Remove(gameObject.GetComponent<AITankController>());
        }

        gameObject.SetActive(false);
    }
    

}

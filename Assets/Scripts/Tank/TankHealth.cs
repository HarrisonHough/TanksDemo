using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour, IDamagable<int>
{
    private int _currentHealth;
    [SerializeField]
    private int _maxHealth = 3;
    public int MaxHealth { get { return _maxHealth; } }

    private bool _isDead = false;

    private GameObject _lastAttacker;
    private float _lastDamageTime = 0;
    [SerializeField]
    private Slider _healthSlider;

    void OnEnable()
    {
        ResetPlayerHealth();
    }

    void ResetPlayerHealth()
    {
        _currentHealth = _maxHealth;
        _healthSlider.maxValue = _currentHealth;
        UpdateHealthBar((int)_healthSlider.maxValue);

        _isDead = false;        
    }

    public void Damage(int damage, GameObject enemyObject)
    {
        if (Time.time - _lastDamageTime < 0.1f)
            return;
        _lastDamageTime = Time.time;
        _currentHealth -= damage;
        Debug.Log("Damage Taken By AI");
        _lastAttacker = enemyObject;
        //TODO fix update health bar
        UpdateHealthBar(_currentHealth);

        if (_currentHealth <= 0 && !_isDead)
        {
            //GameManager.Instance.UpdateScoreboard();
            Die();
        }
    }

    private void UpdateHealthBar(int health)
    {
        _healthSlider.value = health;
    }

    private void Die()
    {
        GameObject explodeFX =  GameManager.Instance.ExplodeFXPool.GetObject();
        explodeFX.transform.position = transform.position;
        explodeFX.SetActive(true);
        _isDead = true;
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

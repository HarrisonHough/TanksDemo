using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private Pool _enemyPool;

    [SerializeField]
    private float _spawnInterval = 10f;

    [SerializeField]
    private Transform[] _spawnPoints;

    private float _lastSpawnTime;

    private bool _isActive;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    public void StopSpawning()
    {
        _isActive = false;
        StopAllCoroutines();
    }

    public void RestartSpawnLoop()
    {
        StartCoroutine( SpawnLoop());
    }

    private Transform GetRandomSpawnPoint()
    {
        return (_spawnPoints[ Random.Range(0,_spawnPoints.Length)]);
    }

    private void SpawnEnemy()
    {
        GameObject newEnemy = _enemyPool.GetObject();
        Transform spawnPoint = GetRandomSpawnPoint();
        newEnemy.transform.position = spawnPoint.position;
        newEnemy.transform.rotation = spawnPoint.rotation;
        newEnemy.SetActive(true);
        GameManager.Instance.ActiveAITanks.Add(newEnemy.GetComponent<AITankController>());
    }

    IEnumerator SpawnLoop()
    {

        yield return new WaitForSeconds(1);
        SpawnEnemy();

        if (!_isActive)
            _isActive = true;
        while (_isActive)
        {
            if (Time.time - _lastSpawnTime > _spawnInterval 
                && GameManager.Instance.ActiveAITanks.Count < 4 
                && GameManager.Instance.Gamestate == GameState.InGame)
            {
                _lastSpawnTime = Time.time;
                SpawnEnemy();
            }
            yield return null;
        }
    }


}

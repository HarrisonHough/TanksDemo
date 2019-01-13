using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private Pool EnemyPool;

    [SerializeField]
    private float spawnInterval = 10f;

    [SerializeField]
    private Transform[] spawnPoints;

    private float lastSpawnTime;

    private bool isActive;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    public void StopSpawning()
    {
        isActive = false;
        StopAllCoroutines();
    }

    public void RestartSpawnLoop()
    {
        StartCoroutine( SpawnLoop());
    }

    private Transform GetRandomSpawnPoint()
    {
        return (spawnPoints[ Random.Range(0,spawnPoints.Length)]);
    }

    private void SpawnEnemy()
    {
        GameObject newEnemy = EnemyPool.GetObject();
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

        if (!isActive)
            isActive = true;
        while (isActive)
        {
            if (Time.time - lastSpawnTime > spawnInterval 
                && GameManager.Instance.ActiveAITanks.Count < 4 
                && GameManager.Instance.Gamestate == GameState.InGame)
            {
                lastSpawnTime = Time.time;
                SpawnEnemy();
            }
            yield return null;
        }
    }


}

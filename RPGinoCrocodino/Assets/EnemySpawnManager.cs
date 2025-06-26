using UnityEngine;
using System.Collections.Generic;

public class EnemySpawnManager : MonoBehaviour
{
    [Header("Препабы врагов")]
    public GameObject meleeEnemyPrefab;
    public GameObject rangedEnemyPrefab;

    [Header("Позиции спавна")]
    public List<Transform> spawnPoints;      

    [Header("Параметры спавна")]
    public int maxEnemiesOnMap = 6;           
    public float spawnInterval = 30f;          

    private int killedEnemiesCount = 0;       


    private void Start()
    {
        InvokeRepeating(nameof(CheckAndSpawn), 0, spawnInterval);

        GameObject[] existingEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in existingEnemies)
        {
            var health = enemy.GetComponent<Health>();
            if (health != null)
            {
                health.SetSpawnManager(this);
            }
        }
    }

    // Метод для проверки, нужно ли добавить врага
    private void CheckAndSpawn()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length < maxEnemiesOnMap)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        
        GameObject enemyPrefab = (Random.value > 0.5f) ? meleeEnemyPrefab : rangedEnemyPrefab;

        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        // Передать в врага ссылку на менеджер, чтобы он сообщил при смерти
        Health enemyHealthScript = enemy.GetComponent<Health>();
        if (enemyHealthScript != null)
        {
            enemyHealthScript.SetSpawnManager(this);
        }
    }

    // Метод, вызываемый врагом при смерти
    public void EnemyKilled(GameObject enemy)
    {
        killedEnemiesCount++;
        Debug.Log("Врагов убито: " + killedEnemiesCount);
    }
}
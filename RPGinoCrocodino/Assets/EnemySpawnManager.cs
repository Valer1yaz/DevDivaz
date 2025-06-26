using UnityEngine;
using System.Collections.Generic;

public class EnemySpawnManager : MonoBehaviour
{
    [Header("������� ������")]
    public GameObject meleeEnemyPrefab;
    public GameObject rangedEnemyPrefab;

    [Header("������� ������")]
    public List<Transform> spawnPoints;      

    [Header("��������� ������")]
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

    // ����� ��� ��������, ����� �� �������� �����
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

        // �������� � ����� ������ �� ��������, ����� �� ������� ��� ������
        Health enemyHealthScript = enemy.GetComponent<Health>();
        if (enemyHealthScript != null)
        {
            enemyHealthScript.SetSpawnManager(this);
        }
    }

    // �����, ���������� ������ ��� ������
    public void EnemyKilled(GameObject enemy)
    {
        killedEnemiesCount++;
        Debug.Log("������ �����: " + killedEnemiesCount);
    }
}
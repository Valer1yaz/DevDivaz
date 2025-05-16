using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class GameDataInteractor
{
    private readonly IGameDataRepository repository;
    private Dictionary<string, GameObject> enemyPrefabs;

    public GameDataInteractor(IGameDataRepository repository)
    {
        this.repository = repository;
    }

    public void SetEnemyPrefabs(Dictionary<string, GameObject> prefabs)
    {
        enemyPrefabs = prefabs;
    }

    public void SaveGame(PlayerController player, List<GameObject> enemies)
    {
        var data = new GameSaveData();

        // Сохраняем параметры игрока
        var playerHealth = player.GetComponent<Health>();
        var playerMagic = player.GetComponent<MagicSystem>();
        data.Player = new PlayerData
        {
            Position = player.transform.position,
            Health = playerHealth.CurrentHealth,
            Mana = playerMagic.CurrentCharges
        };

        // Враги
        List<EnemyData> enemyDataList = new List<EnemyData>();
        foreach (var enemyObj in enemies)
        {
            var enemyIDComponent = enemyObj.GetComponent<EnemyIdentifier>();
            if (enemyIDComponent == null)
            {
                Debug.LogWarning($"Враг {enemyObj.name} не имеет EnemyIdentifier");
                continue;
            }
            string enemyID = enemyIDComponent.EnemyID;

            var enemyHealth = enemyObj.GetComponent<Health>();
            if (enemyHealth == null)
            {
                Debug.LogWarning($"Враг {enemyObj.name} не имеет компонента Health");
                continue;
            }

            enemyDataList.Add(new EnemyData
            {
                EnemyID = enemyID,
                Position = enemyObj.transform.position,
                Health = enemyHealth.CurrentHealth
            });
        }
        data.Enemies = enemyDataList.ToArray();

        repository.Save(data);
    }

    public void LoadGame(PlayerController player, List<GameObject> enemies)
    {
        var data = repository.Load();
        if (data == null)
        {
            Debug.LogWarning("Нет сохраненных данных");
            return;
        }

        // Восстановление игрока
        var playerHealth = player.GetComponent<Health>();
        var playerMagic = player.GetComponent<MagicSystem>();
        player.transform.position = data.Player.Position;
        playerHealth.CurrentHealth = data.Player.Health;
        playerMagic.CurrentCharges = data.Player.Mana;

        // Удаление старых врагов
        foreach (var enemy in enemies)
        {
            if (enemy != null)
                UnityEngine.Object.Destroy(enemy);
        }
        enemies.Clear();

        // Создание новых врагов из данных
        foreach (var enemyData in data.Enemies)
        {
            if (enemyPrefabs != null && enemyPrefabs.TryGetValue(enemyData.EnemyID, out var prefab))
            {
                GameObject newEnemy = UnityEngine.Object.Instantiate(prefab);
                newEnemy.transform.position = enemyData.Position;

                var enemyHealth = newEnemy.GetComponent<Health>();
                if (enemyHealth != null)
                {
                    enemyHealth.CurrentHealth = enemyData.Health;
                }

                enemies.Add(newEnemy);
            }
            else
            {
                Debug.LogWarning($"Не найден префаб врага для ID: {enemyData.EnemyID}");
            }
        }
    }
}
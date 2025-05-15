using System;
using System.Collections.Generic;
using UnityEngine;

public class GameDataInteractor
{
    private readonly IGameDataRepository repository;

    public GameDataInteractor(IGameDataRepository repository)
    {
        this.repository = repository;
    }

    public void SaveGame(PlayerController player, List<GameObject> enemies)
    {
        var data = new GameSaveData();

        // Получаем компоненты
        var playerHealth = player.GetComponent<Health>();
        var playerMagic = player.GetComponent<MagicSystem>();

        // Сохраняем данные игрока
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
            var enemyHealth = enemyObj.GetComponent<Health>();
            string enemyID = enemyObj.name;

            enemyDataList.Add(new EnemyData
            {
                EnemyID = enemyID,
                Position = enemyObj.transform.position,
                Health = enemyHealth.CurrentHealth
            });
        }

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

        // Восстановление врагов
        foreach (var enemyObj in enemies)
        {
            var enemyHealth = enemyObj.GetComponent<Health>();
            string enemyID = enemyObj.name;

            var enemyData = System.Array.Find(data.Enemies, e => e.EnemyID == enemyID);
            if (enemyData != null)
            {
                enemyObj.transform.position = enemyData.Position;
                enemyHealth.CurrentHealth = enemyData.Health;
            }
        }
    }
}
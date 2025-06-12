using System.Collections.Generic;
using UnityEngine;

public class GameBootstrapper : MonoBehaviour
{
    public static GameBootstrapper Instance { get; private set; }

    public IGameDataRepository DataRepository { get; private set; }
    public GameDataInteractor GameInteractor { get; private set; }

    public GameObject EnemyTypeA;
    public GameObject EnemyTypeB;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        InitializeServices();
    }

    private void InitializeServices()
    {
        DataRepository = new JsonGameDataRepository();

        GameInteractor = new GameDataInteractor(DataRepository);

        // Словарь ID -> префаб врага
        var enemyPrefabs = new Dictionary<string, GameObject>
        {
            { "EnemyA", EnemyTypeA },
            { "EnemyB", EnemyTypeB }
        };

        GameInteractor.SetEnemyPrefabs(enemyPrefabs);
    }
}
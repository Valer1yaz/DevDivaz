using UnityEngine;
using System.Collections.Generic;

public class SaveSystemManager : MonoBehaviour
{
    public static SaveSystemManager Instance { get; private set; }

    private GameDataInteractor interactor;

    [SerializeField] private PlayerController player;
    [SerializeField] private List<GameObject> enemies;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // создаем репозиторий и интерктор
        IGameDataRepository repository = new JsonGameDataRepository();
        interactor = new GameDataInteractor(repository);
    }

    public void SaveGame()
    {
        // собираем врагов если не заданы
        if (enemies == null || enemies.Count == 0)
        {
            enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        }
        interactor.SaveGame(player, enemies);
        Debug.Log("Игра сохранена");
    }

    public void LoadGame()
    {
        // собираем врагов
        if (enemies == null || enemies.Count == 0)
        {
            enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        }
        interactor.LoadGame(player, enemies);
        Debug.Log("Игра загружена");
    }
}
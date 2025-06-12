using UnityEngine;
using System.Collections.Generic;

public class SaveSystemManager : MonoBehaviour
{
    public static SaveSystemManager Instance { get; private set; }

    private GameDataInteractor interactor;

    [SerializeField] private PlayerController player;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Получаем интерктор из Bootstrapper
        if (GameBootstrapper.Instance != null)
        {
            interactor = GameBootstrapper.Instance.GameInteractor;
        }
        else
        {
            Debug.LogError("GameBootstrapper не найден!");
        }
    }

    public void SaveGame(PlayerController player)
    {
        // Получить врагов в сцене
        var enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        interactor.SaveGame(player, enemies);
        Debug.Log("Игра сохранена");
    }

    public void LoadGame(PlayerController player)
    {
        var enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        interactor.LoadGame(player, enemies);
        Debug.Log("Игра загружена");
    }
}
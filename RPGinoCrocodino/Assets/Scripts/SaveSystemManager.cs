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

        // ������� ����������� � ���������
        IGameDataRepository repository = new JsonGameDataRepository();
        interactor = new GameDataInteractor(repository);
    }

    public void SaveGame()
    {
        // �������� ������ ���� �� ������
        if (enemies == null || enemies.Count == 0)
        {
            enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        }
        interactor.SaveGame(player, enemies);
        Debug.Log("���� ���������");
    }

    public void LoadGame()
    {
        // �������� ������
        if (enemies == null || enemies.Count == 0)
        {
            enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        }
        interactor.LoadGame(player, enemies);
        Debug.Log("���� ���������");
    }
}
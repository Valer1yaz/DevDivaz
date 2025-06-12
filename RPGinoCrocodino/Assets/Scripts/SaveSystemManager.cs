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

        // �������� ��������� �� Bootstrapper
        if (GameBootstrapper.Instance != null)
        {
            interactor = GameBootstrapper.Instance.GameInteractor;
        }
        else
        {
            Debug.LogError("GameBootstrapper �� ������!");
        }
    }

    public void SaveGame(PlayerController player)
    {
        // �������� ������ � �����
        var enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        interactor.SaveGame(player, enemies);
        Debug.Log("���� ���������");
    }

    public void LoadGame(PlayerController player)
    {
        var enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        interactor.LoadGame(player, enemies);
        Debug.Log("���� ���������");
    }
}
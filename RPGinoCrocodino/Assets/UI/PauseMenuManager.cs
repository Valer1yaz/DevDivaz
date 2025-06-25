using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private Toggle togglePeacefulModeButton;
    [SerializeField] private PlayerController playerController;

    private bool isPaused = false;
    public static bool IsPeacefulModeActive = false;

    private void Start()
    {
        mainMenuButton.onClick.AddListener(ReturnToMainMenu);
        saveButton.onClick.AddListener(SaveGame);
        loadButton.onClick.AddListener(LoadGame);
        togglePeacefulModeButton.onValueChanged.AddListener(TogglePeacefulMode);

        // Изначально меню скрыто
        pauseMenu.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu.activeSelf)
            {
                // Если меню уже открыто, закрываем его
                ResumeGame();
            }
            else
            {
                // Иначе открываем меню
                PauseGame();
            }
        }
    }

    private void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;

        if (playerController != null)
            playerController.enabled = false;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;

        if (playerController != null)
            playerController.enabled = true;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1; // обязательно восстанавливаем время
        SceneManager.LoadScene("MainMenu");
    }

    public void SaveGame()
    {
        // Перед сохранением отключаем игрока, чтобы избежать ошибок
        if (playerController != null)
            playerController.enabled = false;

        // Вызов системы сохранения
        var player = FindObjectOfType<PlayerController>();
        SaveSystemManager.Instance.SaveGame(player);


        Debug.Log("Игра сохранена");
    }

    public void LoadGame()
    {
        // Перед загрузкой отключаем игрока
        if (playerController != null)
            playerController.enabled = false;

        // Вызов системы загрузки
        PlayerController player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        Debug.Log("Перед восстановлением позиции: " + player.transform.position);
        SaveSystemManager.Instance.LoadGame(player);
        
        Debug.Log("После восстановления позиции: " + player.transform.position);
        Debug.Log("Игра загружена");
    }

    public void TogglePeacefulMode(bool isOn)
    {
        IsPeacefulModeActive = isOn;
        Debug.Log("Мирный режим: " + IsPeacefulModeActive);
        UpdateEnemyPeacefulMode(IsPeacefulModeActive);
    }

    private void UpdateEnemyPeacefulMode(bool isPeaceful)
    {
        EnemyAI[] enemies = FindObjectsOfType<EnemyAI>();
        foreach (EnemyAI enemy in enemies)
        {
            enemy.SetPeacefulMode(isPeaceful);
        }
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private PlayerController playerController;

    private bool isPaused = false;

    private void Start()
    {
        mainMenuButton.onClick.AddListener(ReturnToMainMenu);
        saveButton.onClick.AddListener(SaveGame);
        loadButton.onClick.AddListener(LoadGame);

        // »значально меню скрыто
        pauseMenu.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu.activeSelf)
            {
                // ≈сли меню уже открыто, закрываем его
                ResumeGame();
            }
            else
            {
                // »наче открываем меню
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
        Time.timeScale = 1; // об€зательно восстанавливаем врем€
        SceneManager.LoadScene("MainMenu");
    }

    public void SaveGame()
    {
        var player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            SaveSystem.SaveGame(player);
            Debug.Log("Game saved");
        }
        else
        {
            Debug.LogWarning("PlayerController не найден");
        }
    }

    public void LoadGame()
    {
        var player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            SaveSystem.LoadGame(player);
            Debug.Log("Game loaded");
        }
        else
        {
            Debug.LogWarning("PlayerController не найден");
        }
    }
}
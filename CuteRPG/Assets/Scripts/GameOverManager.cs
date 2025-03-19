using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverPanel; // Ссылка на панель "Game Over"
    public Health playerHealth;      // Ссылка на компонент Health героя

    private bool isGameOver = false; // Флаг для отслеживания состояния игры

    void Start()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false); // Скрыть панель при старте
        }
    }

    void Update()
    {
        // Проверяем, умер ли герой
        if (playerHealth != null && playerHealth.currentHealth <= 0 && !isGameOver)
        {
            ShowGameOverPanel();
            isGameOver = true; // Устанавливаем флаг, чтобы панель не показывалась повторно
        }
    }

    void ShowGameOverPanel()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true); // Показать панель "Game Over"
        }

        // Остановить управление героем (если есть компонент PlayerController)
        if (playerHealth.gameObject.TryGetComponent<PlayerController>(out var playerController))
        {
            playerController.enabled = false;
        }
    }

    public void RestartGame()
    {
        // Перезагрузить текущую сцену
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text scoreText;                // UI текст для очков
    public GameObject boss;               // Босс, изначально неактивен
    public AudioSource victoryMusic;      // Победная музыка

    private int score = 0;
    private int enemiesKilled = 0;

    public void AddScore()
    {
        score++;
        enemiesKilled++;
        UpdateScoreUI();

        // Показать боса после 3 убийств
        if (enemiesKilled == 3 && boss != null)
        {
            boss.SetActive(true);
        }

        // Проиграть музыку после 5 убийств
        if (enemiesKilled == 5 && victoryMusic != null)
        {
            victoryMusic.Play();
        }
    }

    private void UpdateScoreUI()
    {
        scoreText.text = "Мобчиков убито: " + score;
    }
}
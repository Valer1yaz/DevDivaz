using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text scoreText;                // UI ����� ��� �����
    public GameObject boss;               // ����, ���������� ���������
    public AudioSource victoryMusic;      // �������� ������

    private int score = 0;
    private int enemiesKilled = 0;

    public void AddScore()
    {
        score++;
        enemiesKilled++;
        UpdateScoreUI();

        // �������� ���� ����� 3 �������
        if (enemiesKilled == 3 && boss != null)
        {
            boss.SetActive(true);
        }

        // ��������� ������ ����� 5 �������
        if (enemiesKilled == 5 && victoryMusic != null)
        {
            victoryMusic.Play();
        }
    }

    private void UpdateScoreUI()
    {
        scoreText.text = "�������� �����: " + score;
    }
}
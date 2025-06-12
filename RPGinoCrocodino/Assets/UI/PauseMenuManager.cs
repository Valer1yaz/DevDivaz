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
    [SerializeField] private PlayerController playerController;

    private bool isPaused = false;

    private void Start()
    {
        mainMenuButton.onClick.AddListener(ReturnToMainMenu);
        saveButton.onClick.AddListener(SaveGame);
        loadButton.onClick.AddListener(LoadGame);

        // ���������� ���� ������
        pauseMenu.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu.activeSelf)
            {
                // ���� ���� ��� �������, ��������� ���
                ResumeGame();
            }
            else
            {
                // ����� ��������� ����
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
        Time.timeScale = 1; // ����������� ��������������� �����
        SceneManager.LoadScene("MainMenu");
    }

    public void SaveGame()
    {
        // ����� ����������� ��������� ������, ����� �������� ������
        if (playerController != null)
            playerController.enabled = false;

        // ����� ������� ����������
        var player = FindObjectOfType<PlayerController>();
        SaveSystemManager.Instance.SaveGame(player);


        Debug.Log("���� ���������");
    }

    public void LoadGame()
    {
        // ����� ��������� ��������� ������
        if (playerController != null)
            playerController.enabled = false;

        // ����� ������� ��������
        PlayerController player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        Debug.Log("����� ��������������� �������: " + player.transform.position);
        SaveSystemManager.Instance.LoadGame(player);
        
        Debug.Log("����� �������������� �������: " + player.transform.position);
        Debug.Log("���� ���������");
    }
}
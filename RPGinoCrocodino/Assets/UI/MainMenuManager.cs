using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private Slider volumeSlider;

    private void Start()
    {
        playButton.onClick.AddListener(StartGame);
        settingsButton.onClick.AddListener(ShowSettings);
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    private void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void ShowSettings()
    {
        settingsPanel.SetActive(true);
    }

    private void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }
}
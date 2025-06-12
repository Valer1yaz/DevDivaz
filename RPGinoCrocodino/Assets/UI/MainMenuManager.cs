using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject Panel;
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button closeSettingsButton;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private Slider volumeSlider;

    private const string VolumePrefKey = "MusicVolume";

    private void Start()
    {
        // Инициализация UI
        playButton.onClick.AddListener(StartGame);
        settingsButton.onClick.AddListener(ShowSettings);
        if (closeSettingsButton != null)
            closeSettingsButton.onClick.AddListener(HideSettings);
        volumeSlider.onValueChanged.AddListener(SetVolume);

        // Загрузка сохранённых настроек
        float savedVolume = PlayerPrefs.GetFloat(VolumePrefKey, 1f);
        volumeSlider.value = savedVolume;
        SetVolume(savedVolume);

        MusicController.Instance.PlayMusic();
    }

    private void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void ShowSettings()
    {
        settingsPanel.SetActive(true);
        Panel.SetActive(false);
    }

    private void HideSettings()
    {
        settingsPanel.SetActive(false);
        Panel.SetActive(true);
        // Сохранение настройки при закрытии
        PlayerPrefs.SetFloat(VolumePrefKey, volumeSlider.value);
        PlayerPrefs.Save();
    }

    private void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat(VolumePrefKey, volume);
    }
}
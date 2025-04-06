using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private Slider hpSlider;
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private Slider magicSlider;
    [SerializeField] private TMP_Text magicText;
    [SerializeField] private GameObject deathScreen;

    private bool isDeathScreenActive = false;

    private void Awake() => Instance = this;

    private void Update()
    {
        // Если экран смерти активен и нажата любая клавиша
        if (isDeathScreenActive && Input.anyKeyDown)
        {
            ReloadScene();
        }
    }

    public void UpdateHealthUI(float currentHP, float maxHP)
    {
        hpSlider.value = currentHP / maxHP;
        hpText.text = $"{currentHP}/{maxHP}";
    }

    public void UpdateMagicUI(int charges, int maxCharges)
    {
        magicSlider.value = (float)charges / maxCharges;
        magicText.text = $"{charges}/{maxCharges}";
    }

    public void ShowDeathScreen()
    {
        deathScreen.SetActive(true);
        isDeathScreenActive = true;

        // Разблокируем курсор
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ReloadScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Health UI")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private Image healthFill;
    [SerializeField] private Gradient healthGradient;

    [Header("Magic UI")]
    [SerializeField] private Slider magicSlider;
    [SerializeField] private TMP_Text magicText;

    [Header("Other References")]
    [SerializeField] private DeathScreen deathScreen;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        healthText.text = $"{currentHealth}/{maxHealth}";
        healthFill.color = healthGradient.Evaluate(healthSlider.normalizedValue);
    }

    public void UpdateMagicUI(int currentCharges, int maxCharges)
    {
        magicSlider.maxValue = maxCharges;
        magicSlider.value = currentCharges;
        magicText.text = $"{currentCharges}/{maxCharges}";
    }

    public void ShowDeathScreen()
    {
        deathScreen.Show();
    }

    public void HideDeathScreen()
    {
        deathScreen.Hide();
    }

    public void OnRespawnButtonClicked()
    {
        GameManager.Instance.ReloadScene();
    }
}
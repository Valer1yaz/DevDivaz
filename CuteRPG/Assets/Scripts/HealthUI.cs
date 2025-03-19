using UnityEngine;
using UnityEngine.UI;
using TMPro; // Добавьте это пространство имён для работы с TextMeshPro

public class HealthUI : MonoBehaviour
{
    public Slider healthSlider;       // Ссылка на слайдер
    public TextMeshProUGUI healthText; // Ссылка на TextMeshProUGUI
    public Health playerHealth;      // Ссылка на компонент Health героя

    void Start()
    {
        if (playerHealth != null && healthSlider != null && healthText != null)
        {
            // Установите максимальное значение слайдера
            healthSlider.maxValue = playerHealth.maxHealth;
            // Установите начальное значение слайдера и текста
            UpdateHealthUI();
        }
    }

    void Update()
    {
        if (playerHealth != null && healthSlider != null && healthText != null)
        {
            // Обновляйте UI каждый кадр
            UpdateHealthUI();
        }
    }
    void UpdateHealthUI()
    {
            healthSlider.value = playerHealth.currentHealth;
            healthText.text = $"{playerHealth.currentHealth}/{playerHealth.maxHealth}";

    // Изменение цвета полосы здоровья
            Image fill = healthSlider.fillRect.GetComponent<Image>();
            if (fill != null)
            {
                if (playerHealth.currentHealth > playerHealth.maxHealth * 0.5f)
                {
                    fill.color = Color.green;
                }
                else if (playerHealth.currentHealth > playerHealth.maxHealth * 0.2f)
                {
                    fill.color = Color.yellow;
                }
                else
                {
                    fill.color = Color.red;
                }
            }
        }
}
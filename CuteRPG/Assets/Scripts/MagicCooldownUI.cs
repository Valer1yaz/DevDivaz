using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MagicCooldownUI : MonoBehaviour
{
    public Slider cooldownSlider; // Ссылка на Slider
    public TextMeshProUGUI cooldownText; // Ссылка на TextMeshPro
    public PlayerCombat playerCombat; // Ссылка на компонент PlayerCombat

    private void Update()
    {
        if (playerCombat != null)
        {
            // Обновление слайдера кулдауна
            float cooldownProgress = (Time.time - playerCombat.LastMagicAttackTime) / playerCombat.magicCooldown;
            cooldownSlider.value = Mathf.Clamp01(cooldownProgress);

            // Обновление текста таймера
            if (cooldownProgress < 1)
            {
                float remainingTime = playerCombat.magicCooldown - (Time.time - playerCombat.LastMagicAttackTime);
                cooldownText.text = $"Cooldown: {remainingTime.ToString("F1")}s";
            }
            else
            {
                cooldownText.text = "Ready!";
            }
        }
    }
}
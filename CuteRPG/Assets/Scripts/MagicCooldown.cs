using UnityEngine;
using UnityEngine.UI; // Добавьте эту директиву
using TMPro;

public class MagicCooldown : MonoBehaviour
{
    public float cooldownTime = 5f; // Время кулдауна в секундах
    public KeyCode magicKey = KeyCode.Q; // Клавиша для использования магии
    public Image magicIcon; // Ссылка на иконку магической атаки
    public TextMeshProUGUI cooldownText; // Ссылка на текстовый элемент кулдауна

    private float cooldownTimer = 0f; // Таймер кулдауна
    private bool isCooldown = false; // Флаг кулдауна

    void Update()
    {
        if (Input.GetKeyDown(magicKey) && !isCooldown)
        {
            // Использовать магическую атаку
            UseMagic();
        }

        if (isCooldown)
        {
            // Обновить таймер кулдауна
            cooldownTimer -= Time.deltaTime;

            // Обновить текст кулдауна
            if (cooldownText != null)
            {
                cooldownText.text = Mathf.Ceil(cooldownTimer).ToString();
            }

            // Проверить, закончился ли кулдаун
            if (cooldownTimer <= 0f)
            {
                EndCooldown();
            }
        }
    }

    void UseMagic()
    {
        // Логика использования магической атаки
        Debug.Log("Магическая атака использована!");

        // Начать кулдаун
        StartCooldown();
    }

    void StartCooldown()
    {
        isCooldown = true;
        cooldownTimer = cooldownTime;

        // Затемнить иконку (опционально)
        if (magicIcon != null)
        {
            magicIcon.color = new Color(0.5f, 0.5f, 0.5f, 0.5f); // Затемнение
        }
    }

    void EndCooldown()
    {
        isCooldown = false;

        // Сбросить текст кулдауна
        if (cooldownText != null)
        {
            cooldownText.text = "";
        }

        // Восстановить иконку (опционально)
        if (magicIcon != null)
        {
            magicIcon.color = Color.white; // Вернуть нормальный цвет
        }
    }
}
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public int physicalDamage = 10;
    public int magicDamage = 15;
    public float magicCooldown = 5f;

    // Приватное поле для хранения времени последней магической атаки
    private float _lastMagicAttackTime = -Mathf.Infinity;

    // Публичное свойство для доступа к _lastMagicAttackTime
    public float LastMagicAttackTime => _lastMagicAttackTime;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Правая кнопка мыши
        {
            if (Time.time - _lastMagicAttackTime >= magicCooldown)
            {
                PerformMagicAttack();
                _lastMagicAttackTime = Time.time; // Обновляем время последней атаки
            }
        }
    }

    private void PerformMagicAttack()
    {
        // Логика магической атаки
        Debug.Log("Магическая атака!");
    }
}
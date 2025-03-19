using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public int physicalDamage = 10;
    public int magicDamage = 15;
    public float magicCooldown = 5f;
    private float lastMagicAttackTime;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Левая кнопка мыши
        {
            PerformPhysicalAttack();
        }
        if (Input.GetMouseButtonDown(1)) // Правая кнопка мыши
        {
            if (Time.time - lastMagicAttackTime >= magicCooldown)
            {
                PerformMagicAttack();
                lastMagicAttackTime = Time.time;
            }
        }
    }

    private void PerformPhysicalAttack()
    {
        // Логика физической атаки
    }

    private void PerformMagicAttack()
    {
        // Логика магической атаки
    }
}
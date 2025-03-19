using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Animator animator;
    public GameObject gameOverUI; // Ссылка на UI "Game Over"

    private void Start()
    {
        currentHealth = maxHealth;
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false); // Скрыть UI при старте
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            animator.SetTrigger("TakeDamage"); // Анимация получения урона
        }
    }

    private void Die()
    {
        animator.SetTrigger("Die"); // Анимация смерти
        if (gameObject.CompareTag("Player"))
        {
            // Остановить управление и показать экран "Game Over"
            GetComponent<PlayerController>().enabled = false;
            if (gameOverUI != null)
            {
                gameOverUI.SetActive(true);
            }
        }
        else
        {
            // Удалить моба из сцены через 2 секунды после смерти
            Destroy(gameObject, 2f);
        }
    }
}
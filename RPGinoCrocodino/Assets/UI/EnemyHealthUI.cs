using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    private Health health;

    private void Start()
    {
        health = GetComponentInParent<Health>();
        healthSlider.value = 1;
    }

    private void Update()
    {
        // Поворот к камере
        transform.rotation = Camera.main.transform.rotation;

        // Обновление полосы
        healthSlider.value = health.currentHP / health.maxHP;
    }
}
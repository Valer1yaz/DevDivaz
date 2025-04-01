using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Vector3 offset = new Vector3(0, 2f, 0);

    private Transform target;
    private Camera mainCamera;

    public void Initialize(int maxHealth)
    {
        mainCamera = Camera.main;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
        target = transform.parent;
        transform.SetParent(null);
    }

    public void UpdateHealth(int currentHealth)
    {
        healthSlider.value = currentHealth;
    }

    private void LateUpdate()
    {
        if (target != null && mainCamera != null)
        {
            transform.position = target.position + offset;
            transform.rotation = mainCamera.transform.rotation;
        }
    }
}
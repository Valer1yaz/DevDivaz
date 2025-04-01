using UnityEngine;
using System.Collections;

public class HealingHerb : MonoBehaviour
{
    [SerializeField] private int healAmount = 20;
    [SerializeField] private float respawnTime = 30f;

    private Collider collider;
    private MeshRenderer meshRenderer;
    private bool isActive = true;

    private void Awake()
    {
        collider = GetComponent<Collider>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActive) return;

        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent(out HeroStats heroStats))
            {
                heroStats.Heal(healAmount);
                StartCoroutine(Respawn());
            }
        }
    }

    private IEnumerator Respawn()
    {
        isActive = false;
        collider.enabled = false;
        meshRenderer.enabled = false;

        yield return new WaitForSeconds(respawnTime);

        isActive = true;
        collider.enabled = true;
        meshRenderer.enabled = true;
    }
}
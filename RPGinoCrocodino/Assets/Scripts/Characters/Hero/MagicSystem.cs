using UnityEngine;

public class MagicSystem : MonoBehaviour
{
    [SerializeField] private int maxMagicCharges = 3;
    [SerializeField] private float rechargeCooldown = 5f;
    [SerializeField] private float rechargeRate = 0.5f;

    private int currentCharges;
    private float rechargeTimer;
    private bool isRecharging;

    public int CurrentCharges => currentCharges;
    public int MaxCharges => maxMagicCharges;

    private void Awake()
    {
        currentCharges = maxMagicCharges;
    }

    private void Update()
    {
        HandleRecharge();
    }

    public bool HasEnoughMagic()
    {
        return currentCharges > 0;
    }

    public void ConsumeMagic(int amount)
    {
        currentCharges = Mathf.Max(currentCharges - amount, 0);
        rechargeTimer = rechargeCooldown;
        isRecharging = true;

        UIManager.Instance.UpdateMagicUI(currentCharges, maxMagicCharges);
    }

    private void HandleRecharge()
    {
        if (!isRecharging || currentCharges >= maxMagicCharges) return;

        rechargeTimer -= Time.deltaTime;

        if (rechargeTimer <= 0)
        {
            currentCharges = Mathf.Min(currentCharges + 1, maxMagicCharges);
            rechargeTimer = rechargeRate;

            UIManager.Instance.UpdateMagicUI(currentCharges, maxMagicCharges);

            if (currentCharges >= maxMagicCharges)
            {
                isRecharging = false;
            }
        }
    }
}
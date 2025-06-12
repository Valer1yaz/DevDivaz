using UnityEngine;

public class MagicSystem : MonoBehaviour
{
    [SerializeField] public int maxCharges = 5;
    [SerializeField] public float rechargeTime = 5f;
    public int currentCharges;
    private float rechargeTimer;

    private void Start()
    {
        currentCharges = maxCharges;
        UIManager.Instance.UpdateMagicUI(currentCharges, maxCharges);
    }

    private void Update()
    {
        if (currentCharges < maxCharges)
        {
            rechargeTimer += Time.deltaTime;
            if (rechargeTimer >= rechargeTime)
            {
                currentCharges++;
                rechargeTimer = 0f;
                UIManager.Instance.UpdateMagicUI(currentCharges, maxCharges);
            }
        }
    }

    public int CurrentCharges
    {
        get => currentCharges;
        set
        {
            currentCharges = Mathf.Clamp(value, 0, maxCharges);
            UIManager.Instance.UpdateMagicUI(currentCharges, maxCharges);
        }
    }

    public bool CanUseMagic() => currentCharges > 0;

    public void UseMagic()
    {
        currentCharges--;
        UIManager.Instance.UpdateMagicUI(currentCharges, maxCharges);
    }
}
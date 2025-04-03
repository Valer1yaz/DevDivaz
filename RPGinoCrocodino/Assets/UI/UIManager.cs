using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private Slider hpSlider;
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private Slider magicSlider;
    [SerializeField] private TMP_Text magicText;
    [SerializeField] private GameObject deathScreen;

    private void Awake() => Instance = this;

    public void UpdateHealthUI(float currentHP, float maxHP)
    {
        hpSlider.value = currentHP / maxHP;
        hpText.text = $"{currentHP}/{maxHP}";
    }

    public void UpdateMagicUI(int charges, int maxCharges)
    {
        magicSlider.value = (float)charges / maxCharges;
        magicText.text = $"{charges}/{maxCharges}";
    }
    public void ShowDeathScreen() => deathScreen.SetActive(true);

    public void ReloadScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}
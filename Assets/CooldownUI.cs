using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CooldownUI : MonoBehaviour
{
    public Image cooldownFill;
    public TextMeshProUGUI cooldownText;
    public PlayerMovement player;

    void Update()
    {
            UpdateCooldownUI();
    }

    private void UpdateCooldownUI()
    {
        float fillAmount = player.rightAttackCooldownTimer / player.rightAttackCooldown;
        cooldownFill.fillAmount = fillAmount;
        cooldownText.text = Mathf.Ceil(player.rightAttackCooldownTimer).ToString();
    }
}

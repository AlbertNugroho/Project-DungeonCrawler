using System.Collections;
using UnityEngine;

public class RightAttack : IAttackSkill
{
    int staminacost = 30;
    public void ExecuteAttack(PlayerMovement player, Vector2 direction)
    {
        if (player.StaminaBar.staminaBar.value < staminacost)
        {
            return;
        }
        else
        {
            player.StaminaBar.useStamina(staminacost);
        }

        player.cc.isTrigger = true;
        player.StartDash(direction, 40);
        CombatManager.instance.inputrecived = true;
        player.StartCoroutine(Playdashsound(player));
        player.rightAttackCooldownTimer = player.rightAttackCooldown;
    }

    IEnumerator Playdashsound(PlayerMovement player)
    {
        player.am.playclip(player.am.dashfx);
        yield return new WaitForSeconds(0.1f);
        player.am.playclip(player.am.slashfx);
    }
}

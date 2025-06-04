using System.Collections;
using UnityEngine;

public class BasicAttack : IAttackSkill
{
    int staminacost = 20;
    int Damage = 20;
    public void ExecuteAttack(PlayerMovement player, Vector2 direction)
    {
        player.Damage = Damage;
        if (CombatManager.instance.canreciveinput)
        {
            if (player.StaminaBar.staminaBar.value < staminacost)
            {
                return;
            }
            else
            {
                player.StaminaBar.useStamina(staminacost);
            }
            CombatManager.instance.canreciveinput = false;
            CombatManager.instance.inputrecived = true;
            player.am.playclip(player.am.slashfx);
            player.a.SetTrigger("Dashing");
        }
        player.leftAttackCooldownTimer = player.leftAttackCooldown;
    }
}
using System.Collections;
using UnityEngine;

public class RightAttack : IAttackSkill
{
    int staminacost = 30;
    int Damage = 30;
    int cooldown = 7;
    public void ExecuteAttack(PlayerMovement player, Vector2 direction)
    {
        player.Damage = Damage;
        if (player.StaminaBar.staminaBar.value < staminacost)
        {
            return;
        }
        else
        {
            player.StaminaBar.useStamina(staminacost);
        }
        player.StartCoroutine(PlayAnimation(player, direction));
        player.rightAttackCooldownTimer = cooldown;
    }

    IEnumerator PlayAnimation(PlayerMovement player, Vector2 direction)
    {
        CompositeCollider2D platformCollider = GameObject.FindGameObjectWithTag("OneWayPlatform").GetComponent<CompositeCollider2D>();
        Physics2D.IgnoreCollision(player.cc, platformCollider, true);
        EnableCircleHitbox.EnableDashDamageColider();
        player.StartDash(direction, 40);
        CombatManager.instance.inputrecived = true;
        player.StartCoroutine(Playdashsound(player));
        yield return new WaitForSeconds(1);
        EnableCircleHitbox.DisableDashDamageColider();
        Physics2D.IgnoreCollision(player.cc, platformCollider, false);
    }


    IEnumerator Playdashsound(PlayerMovement player)
    {
        player.am.playclip(player.am.dashfx);
        yield return new WaitForSeconds(0.1f);
        player.am.playclip(player.am.slashfx);
    }
}

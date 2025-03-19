using System.Collections;
using UnityEngine;

public class OmniDirectionalSlash : IAttackSkill
{
    int staminacost = 70;
    int Damage = 10;
    int cooldown = 15;
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
        player.am.playclip(player.am.dashfx);
        player.isBusy = true;
        player.Scythe.Play("Base Layer.Start", 0, 0f);
        player.StartDash(direction, 30);
        yield return new WaitForSeconds(player.dashTime);
        player.rb.bodyType = RigidbodyType2D.Static;
        player.Scythe.Play("Base Layer.Scythe_idle", 0  , 0f);
        player.a.Play("Base Layer.Omnislash", 0, 0f);
        PlaySound(player);
        yield return new WaitForSeconds(0.1f);
        PlaySound(player);
        yield return new WaitForSeconds(0.1f);
        PlaySound(player);
        yield return new WaitForSeconds(0.1f);
        PlaySound(player);
        yield return new WaitForSeconds(0.1f);
        PlaySound(player);
        yield return new WaitForSeconds(0.55f);
        player.rb.bodyType = RigidbodyType2D.Dynamic;
        player.am.playclip(player.am.dashfx);
        player.StartDash(direction, 30);
        player.Scythe.Play("Base Layer.Attack1", 0, 0f);
        CombatManager.instance.canreciveinput = true;
    }

    IEnumerator PlaySound(PlayerMovement player)
    {
        player.am.playclip(player.am.slashfx);
        yield return new WaitForSeconds(0.1f);
        player.am.playclip(player.am.slashfx);
        yield return new WaitForSeconds(0.1f);
        player.am.playclip(player.am.slashfx);
        yield return new WaitForSeconds(0.1f);
        player.am.playclip(player.am.slashfx);
        yield return new WaitForSeconds(0.1f);
        player.am.playclip(player.am.slashfx);
        yield return new WaitForSeconds(0.1f);
        player.am.playclip(player.am.slashfx);
        yield return new WaitForSeconds(0.1f);
        player.am.playclip(player.am.slashfx);
        yield return new WaitForSeconds(0.1f);
        player.am.playclip(player.am.slashfx);
        yield return new WaitForSeconds(0.1f);
        player.am.playclip(player.am.slashfx);
        yield return new WaitForSeconds(0.1f);
        player.am.playclip(player.am.slashfx);
    }
}

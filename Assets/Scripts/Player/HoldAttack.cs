using System;
using System.Collections;
using UnityEngine;

public class HoldAttack : IChargeAttack
{
    int staminacost = 50;
    int Damage = 30;
    float t;
    public void ChargeAttack(PlayerMovement player)
    {
        if (!player.leftAttackTransitionPlayed && player.leftAttackHoldTimer >= player.leftAttackTransitionThreshold)
        {
            player.leftAttackTransitionPlayed = true;
            player.Scythe.Play("Base Layer.HoldAttack", 0, 0f);
        }
        else if (player.leftAttackTransitionPlayed && !player.scythemaxsize)
        {
            t = Mathf.Clamp01((player.leftAttackHoldTimer - player.leftAttackTransitionThreshold) / (player.leftAttackRequiredHoldTime - player.leftAttackTransitionThreshold));
            player.SetScytheScale(Vector3.Lerp(player.BaseScytheScale, player.BaseScytheScale * player.scytheTargetScaleMultiplier, t));

            if (t >= 1.0f)
            {
                player.scythemaxsize = true;
                player.Scythe.Play("Base Layer.Holdattackhold", 0, 0f);
            }
        }
    }

    public void ExecuteAttack(PlayerMovement player, Vector2 direction)
    {
        player.Damage = Damage * (int)t * 2;
        if (player.StaminaBar.staminaBar.value < staminacost)
        {
            player.Scythe.Play("Base Layer.cancelcharge");
            return;
        }
        else
        {
            player.StaminaBar.useStamina(staminacost);
        }
        if (player.leftAttackHoldTimer >= player.leftAttackRequiredHoldTime)
        {
            player.Scythe.SetTrigger("Charged");
            player.StartDash(direction, 30);
        }
        else
        {
            player.Scythe.SetTrigger("Charged");
            player.a.SetTrigger("Dashing");
        }
        player.am.playclip(player.am.slashfx);
        float multiplier;
        if(player.leftAttackHoldTimer > player.leftAttackRequiredHoldTime)
        {
            multiplier = player.leftAttackRequiredHoldTime;
        }
        else
        {
            multiplier = player.leftAttackHoldTimer;
        }
        PlayerHealth.ShakeCamera(2f * multiplier, 0.5f);
        player.leftAttackCooldownTimer = player.leftAttackCooldown;
    }
}
using System.Collections;
using UnityEngine;


public interface IAttackSkill
{
    void ExecuteAttack(PlayerMovement player, Vector2 direction);
}

public interface IChargeAttack : IAttackSkill
{
    void ChargeAttack(PlayerMovement player);
}
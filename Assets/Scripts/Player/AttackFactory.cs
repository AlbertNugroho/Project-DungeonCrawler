
public class AttackFactory
{
    public IAttackSkill GetBasicAttack() => new BasicAttack();
    public IChargeAttack GetHoldAttack() => new HoldAttack();
    public IAttackSkill GetRightAttack() => new RightAttack();

    public IAttackSkill GetOmniDirectionalSlash() => new OmniDirectionalSlash();
}
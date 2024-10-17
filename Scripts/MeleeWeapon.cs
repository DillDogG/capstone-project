using Godot;
using System;

public partial class MeleeWeapon : Weapon
{
    public int ComboCount { get; set; }
    public int CurrComboCount { get; set; } = 0;
    public double ComboMult { get; set; }
    public double ComboGracePeriod { get; set; }
    public double AttackDuration { get; set; }
    public double MaxAttackDuration { get; set; }

    public override void MainUpdate(double delta)
    {
        base.MainUpdate(delta);
        if (ComboGracePeriod > 0) ComboGracePeriod -= delta;
        else CurrComboCount = 0;
        if (AttackDuration > 0)
        {
            AttackDuration -= delta;
            if (HitCheck.HasOverlappingBodies())
            {
                foreach (var hits in HitCheck.GetOverlappingBodies())
                {
                    if (hits is Damageable)
                    {
                        Damageable damageable = hits as Damageable;
                        if (CurrComboCount >= ComboCount) damageable.ApplyDamage(Damage * ComboMult);
                        else damageable.ApplyDamage(Damage);
                    }
                }
            }
        }
    }

    public override void Attack()
    {
        if (FireCooldown > 0) return;
        if (ComboGracePeriod > 0)
        {
            CurrComboCount++;
        }
    }
}

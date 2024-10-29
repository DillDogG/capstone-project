using Godot;
using System;

public partial class MeleeWeapon : Weapon
{
    [Export]
    public int ComboCount { get; set; }
    public int CurrComboCount { get; set; } = 0;
    [Export]
    public double ComboMult { get; set; } = 1;
    [Export]
    public double ComboGracePeriod { get; set; } = 2;

    public double ComboGraceTracker { get; set; }
    public double AttackDuration { get; set; }
    [Export]
    public double MaxAttackDuration { get; set; } = 4;
    [Export]
    public virtual Area3D HitCheck { get; set; }

    public override void MainUpdate(double delta)
    {
        base.MainUpdate(delta);
        if (ComboGraceTracker > 0) ComboGraceTracker -= delta;
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
                        if (CurrComboCount >= ComboCount)
                        {
                            damageable.ApplyDamage(Damage * ComboMult);
                            CurrComboCount = 0;
                        }
                        else damageable.ApplyDamage(Damage);
                    }
                }
            }
        }
    }

    public override void Attack()
    {
        if (FireCooldown > 0) return;
        if (ComboGraceTracker > 0)
        {
            CurrComboCount++;
        }
        AttackDuration = MaxAttackDuration;
        FireCooldown = FireRate;
        ComboGraceTracker = ComboGracePeriod;
    }
}

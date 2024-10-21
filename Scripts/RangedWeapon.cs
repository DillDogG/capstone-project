using Godot;
using System;

public partial class RangedWeapon : Weapon
{
    public int AmmoCount { get; set; }
    public int MaxAmmoCount { get; set; }
    public double ReloadSpeed { get; set; }
    public bool Reloading { get; set; } = false;

    public override void MainUpdate(double delta)
    {
        base.MainUpdate(delta);
        if (FireCooldown <= 0 && Reloading)
        {
            AmmoCount = MaxAmmoCount;
            Reloading = false;
        }
    }

    public override void Attack()
    {
        if (AmmoCount <= 0)
        {
            Reload();
            return;
        }
        base.Attack();
        if (HitCheck.HasOverlappingBodies())
        {
            if (HitCheck.GetOverlappingBodies()[0] is Damageable)
            {
                Damageable damageable = HitCheck.GetOverlappingBodies()[0] as Damageable;
                damageable.ApplyDamage(Damage);
            }
        }
        FireCooldown = FireRate;
        AmmoCount--;
    }

    public void Reload()
    {
        if (AmmoCount == MaxAmmoCount) return;
        Reloading = true;
        FireCooldown = ReloadSpeed;
    }
}

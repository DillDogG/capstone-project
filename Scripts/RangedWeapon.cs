using Godot;
using System;

public partial class RangedWeapon : Weapon
{
    public int AmmoCount { get; set; }
    [Export]
    public int MaxAmmoCount { get; set; }
    [Export]
    public double ReloadSpeed { get; set; }
    [Export]
    public double AutoReloadSpeed { get; set; }
    public bool Reloading { get; set; } = false;
    public double AutoReloadTime { get; set; }
    public virtual RayCast3D HitCheck { get; set; }
    public TimerLabel GlobalTimer { get; set; }

    public override void MainUpdate(double delta)
    {
        base.MainUpdate(delta);
        if (FireCooldown <= 0 && Reloading)
        {
            AmmoCount = MaxAmmoCount;
            Reloading = false;
        }
    }

    public override void Equip()
    {
        base.Equip();
        if (AutoReloadTime + AutoReloadSpeed + ReloadSpeed - GlobalTimer._timer <= 0)
        {
            if (AmmoCount + 2 > MaxAmmoCount - 2) AmmoCount += 2;
            else AmmoCount = MaxAmmoCount - 2;
            if (AmmoCount > MaxAmmoCount) AmmoCount = MaxAmmoCount;
        }
    }

    public override void Unequip()
    {
        base.Unequip();
        AutoReloadTime = GlobalTimer._timer;
    }

    public override void Attack()
    {
        if (Reloading) return;
        if (AmmoCount <= 0)
        {
            Reload();
            return;
        }
        if (FireCooldown > 0) return;
        if (HitCheck.IsColliding())
        {
            if (HitCheck.GetCollider() is Damageable)
            {
                Damageable damageable = HitCheck.GetCollider() as Damageable;
                damageable.ApplyDamage(Damage);
            }
        }
        FireCooldown = FireRate;
        AmmoCount--;
    }

    public void Reload(double TimeAddition = 0)
    {
        if (AmmoCount == MaxAmmoCount) return;
        Reloading = true;
        FireCooldown = ReloadSpeed + TimeAddition;
    }
}

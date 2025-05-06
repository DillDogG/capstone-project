using Godot;
using System;

public partial class RocketLauncher : RangedWeapon
{
    [Export]
    AnimationPlayer animation;

    [Export]
    PackedScene RocketPrefab;
    public RocketLauncher()
    {
        if (MaxAmmoCount == 0) MaxAmmoCount = 1;
        if (MaxAmmoReserves == 0) MaxAmmoReserves = 7;
        if (ReloadSpeed == 0) ReloadSpeed = 6;
        if (AutoReloadSpeed == 0) AutoReloadSpeed = 15;
        FireType = FireEnum.SINGLE;
        if (FireRate == 0) FireRate = 1.5;
    }

    public override void Attack()
    {
        int startAmmo = AmmoCount;
        //base.Attack();
        if (FireType == FireEnum.FULLAUTO) Firing = true;
        if (Reloading) return;
        if (AmmoCount <= 0)
        {
            Reload();
            return;
        }
        if (FireCooldown > 0) return;
        if (HitCheck.IsColliding())
        {
            RocketAmmo rocket = RocketPrefab.Instantiate<RocketAmmo>();

        }
        FireCooldown = FireRate;
        AmmoCount--;
        if (AmmoCount != startAmmo) animation.Play("Shoot");
    }
}

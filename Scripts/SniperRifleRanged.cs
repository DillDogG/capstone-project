using Godot;
using System;

public partial class SniperRifleRanged : RangedWeapon
{
    [Export]
    AnimationPlayer animation;
    public SniperRifleRanged()
    {
        if (MaxAmmoCount == 0) MaxAmmoCount = 7;
        if (MaxAmmoReserves == 0) MaxAmmoReserves = 35;
        if (ReloadSpeed == 0) ReloadSpeed = 3.5;
        if (AutoReloadSpeed == 0) AutoReloadSpeed = 5;
        FireType = FireEnum.FULLAUTO;
        if (Damage == 5) Damage = 70;
        if (FireRate == 0) FireRate = 1;
    }

    public override void Attack()
    {
        int startAmmo = AmmoCount;
        base.Attack();
        if (AmmoCount != startAmmo) animation.Play("Shoot");
    }
}

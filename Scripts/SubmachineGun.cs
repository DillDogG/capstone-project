using Godot;
using System;

public partial class SubmachineGun : RangedWeapon
{
    [Export]
    AnimationPlayer animation;
    public SubmachineGun()
    {
        if (MaxAmmoCount == 0) MaxAmmoCount = 35;
        if (MaxAmmoReserves == 0) MaxAmmoReserves = 175;
        if (ReloadSpeed == 0) ReloadSpeed = 3.5;
        if (AutoReloadSpeed == 0) AutoReloadSpeed = 5;
        FireType = FireEnum.FULLAUTO;
        if (Damage == 5) Damage = 15;
        if (FireRate == 0) FireRate = 0.0625;
    }

    public override void Attack()
    {
        int startAmmo = AmmoCount;
        base.Attack();
        if (AmmoCount != startAmmo) animation.Play("Shoot");
    }
}

using Godot;
using System;

public partial class RocketLauncher : RangedWeapon
{
    [Export]
    AnimationPlayer animation;

    [Export]
    Area3D explosion;

    [Export]
    Area3D explosionOuter;
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
            //var origin = HitCheck.GlobalTransform.Origin;
            //var collision_point = HitCheck.GetCollisionPoint();
            //var distance = origin.DistanceTo(collision_point);
            //GD.Print(distance);
            //GD.Print(Vector3.Forward * distance);
            //Node3D hitObject = (Node3D)HitCheck.GetCollider();
            //explosion.Transform.Translated(Vector3.Forward * distance);
            //explosion.Position = (Vector3.Forward * distance);
            //explosionOuter.Transform.Translated(Vector3.Forward * distance);
            //explosionOuter.Position = (Vector3.Forward * distance);
            foreach (var hits in explosionOuter.GetOverlappingBodies())
            {
                if (hits is Damageable)
                {
                    Damageable damageable = hits as Damageable;
                    
                    if (explosion.GetOverlappingBodies().Contains(hits)) damageable.ApplyDamage(200);
                    else damageable.ApplyDamage(75);
                }
            }
        }
        FireCooldown = FireRate;
        AmmoCount--;
        if (AmmoCount != startAmmo) animation.Play("Shoot");
    }
}

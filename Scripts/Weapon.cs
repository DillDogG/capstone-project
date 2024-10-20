using Godot;
using System;

public partial class Weapon : Node
{
    // Used for equip animation
    public enum AnimType { MELEE, ONEHANDED, TWOHANDED };
    public enum FireType { SINGLE, FULLAUTO };
    [Export]
    public virtual double Damage { get; set; } = 5;
    [Export]
    public virtual double FireRate { get; set; }
    public virtual double FireCooldown { get; set; } = 0;
    [Export]
    public virtual bool IsEquipped { get; set; }
    // Used for the hitbox that the weapon has
    [Export]
    public virtual Area3D HitCheck { get; set; }

    // used for all update purposes
    public virtual void MainUpdate(double delta)
    {
        if (FireCooldown > 0) FireCooldown -= delta;
    }

    public virtual void Equip()
    {
        IsEquipped = true;
    }

    public virtual void Unequip()
    {
        IsEquipped = false;
    }

    public virtual void Attack()
    {
        if (FireCooldown > 0) return;
    }

}

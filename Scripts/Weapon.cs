using Godot;
using System;

public partial class Weapon : Node
{
    // Used for equip animation
    public enum AnimType { MELEE, ONEHANDED, TWOHANDED };
    public enum FireType { SINGLE, FULLAUTO };
    public virtual double Damage { get; set; }
    public virtual double FireRate { get; set; }
    public virtual double FireCooldown { get; set; } = 0;
    public virtual bool IsEquipped { get; set; }
    // Used for the hitbox that the weapon has
    public virtual Area3D HitCheck { get; set; }

    // used for all update purposes
    public virtual void MainUpdate(double delta)
    {
        if (FireRate > 0) FireRate -= delta;
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

using Godot;
using System;

public partial class Weapon : Node
{
    // Used for equip animation
    public enum AnimType { MELEE, ONEHANDED, TWOHANDED };
    public virtual double Damage { get; set; }
    public virtual double FireRate { get; set; }
    public virtual bool IsEquipped { get; set; }
    // Used for the hitbox that the weapon has
    public virtual Node3D HitCheck { get; set; }

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

    }
}

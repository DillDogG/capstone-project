using Godot;
using System;

public partial class Weapon : Node
{
    // Used for equip animation
    //public enum AnimEnum { MELEE, ONEHANDED, TWOHANDED };
    public enum FireEnum { SINGLE, FULLAUTO };
    //[Export]
    //public AnimEnum AnimType { get; set; }
    [Export]
    public FireEnum FireType { get; set; }
    public bool Firing { get; set; } = false;
    [Export]
    public virtual double Damage { get; set; } = 5;
    [Export]
    public virtual double FireRate { get; set; }
    public virtual double FireCooldown { get; set; } = 0;
    public virtual bool IsEquipped { get; set; }
    // Used for the hitbox that the weapon has

    [Export]
    MeshInstance3D Model;

    // used for all update purposes
    public virtual void MainUpdate(double delta)
    {
        if (FireCooldown > 0) FireCooldown -= delta;
        if (Firing) Attack();
    }

    public virtual void Equip()
    {
        IsEquipped = true;
        if (Model != null) Model.Visible = true;
    }

    public virtual void Unequip()
    {
        IsEquipped = false;
        if (Model != null) Model.Visible = false;
    }

    public virtual void Attack() { }

    public virtual void EndAttack()
    {
        Firing = false;
    }
}

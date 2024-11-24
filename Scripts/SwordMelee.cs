using Godot;
using System;
using System.Collections.Generic;

public partial class SwordMelee : MeleeWeapon
{
    [Export]
    AnimationPlayer animation;
    public SwordMelee()
    {
        Hits = new List<Node3D>();
        if (ComboCount == 0) ComboCount = 3;
        if (ComboMult == 1) ComboMult = 2;
        if (ComboGracePeriod == 2) ComboGracePeriod = 2.5;
        if (MaxAttackDuration == 4) MaxAttackDuration = 0.75;
        //AnimType = AnimEnum.MELEE;
        FireType = FireEnum.SINGLE;
        if (Damage == 5) Damage = 35;
        if (FireRate == 0) FireRate = 1.25;
        //animation = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    public override void Attack()
    {
        base.Attack();
        animation.Play("SwordSwing");
    }
}

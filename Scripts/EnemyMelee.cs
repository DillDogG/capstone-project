using Godot;
using System;
using System.Collections.Generic;

public partial class EnemyMelee : MeleeWeapon
{
    public EnemyMelee()
    {
        Hits = new List<Node3D>();
        if (ComboCount == 0) ComboCount = 3;
        if (ComboMult == 1) ComboMult = 1.25;
        if (ComboGracePeriod == 2) ComboGracePeriod = 3.5;
        if (MaxAttackDuration == 4) MaxAttackDuration = 1.5;
        //AnimType = AnimEnum.MELEE;
        FireType = FireEnum.SINGLE;
        if (Damage == 5) Damage = 20;
        FireRate = 2.6333;
        if (Delay == 0) Delay = .8;
    }
}

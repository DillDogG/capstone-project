using Godot;
using System;

public partial class Enemy : CharacterBody3D, Damageable
{
    [Export]
    public int Speed { get; set; } = 20;

    [Export]
    public double MaxHealth = 150;

    [Export]
    public double InvincibilityDuration = 2;

    private double Health { get; set; }

    private double InvincibilityTime { get; set; }

    public override void _Ready()
    {
        Health = MaxHealth;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (InvincibilityTime > 0) InvincibilityTime -= delta;
        MoveAndSlide();
    }

    public void ApplyDamage(double damage)
    {
        if (InvincibilityTime > 0) return;
        Health -= damage;
        InvincibilityTime = InvincibilityDuration;
        GD.Print(Health);
        if (Health <= 0)
        {
            // kill method
        }
    }
}

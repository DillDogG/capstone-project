using Godot;
using System;

public partial class RocketAmmo : Node3D
{
    [Export]
    Area3D explosionCollider;

    [Export]
    Area3D explosion;

    [Export]
    Area3D explosionOuter;

    [Export]
    float velocity;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        foreach (var hit in explosionCollider.GetOverlappingBodies())
        {
            if (hit is Damageable)
            {
                foreach (var hits in explosionOuter.GetOverlappingBodies())
                {
                    if (hits is Damageable)
                    {
                        Damageable damageable = hits as Damageable;

                        if (explosion.GetOverlappingBodies().Contains(hits)) damageable.ApplyDamage(200);
                        else damageable.ApplyDamage(75);
                    }
                }
                QueueFree();
            }
            else
            {
                QueueFree();
            }
        }
        
    }
}

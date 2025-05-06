using Godot;
using System;

public partial class RocketAmmo : RigidBody3D
{
    [Export]
    Area3D explosion;

    [Export]
    Area3D explosionOuter;

    [Export]
    public float velocity;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
       
    }

    public void Initialize(Vector3 startPosition)
    {
        Position = startPosition;
        RotateY(Mathf.DegToRad(-90));
        LinearVelocity = GlobalTransform.Basis * new Vector3(0, 0, -velocity);
        RotateY(Mathf.DegToRad(90));
        RotateZ(Mathf.DegToRad(90));
        GD.Print("rocket");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
	{
        foreach (var hit in GetCollidingBodies())
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
        }
        
    }
}

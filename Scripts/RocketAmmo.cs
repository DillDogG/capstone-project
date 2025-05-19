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

    [Export]
    public float lifeSpan = 5;

    private float lifeTime;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        lifeTime = lifeSpan;
    }

    public void Initialize(Vector3 startPosition)
    {
        Position = startPosition;
        Rotation = new Vector3(-Rotation.X, Rotation.Y, Rotation.Z);
        RotateZ(Mathf.DegToRad(-5));
        RotateY(Mathf.DegToRad(-90));
        LinearVelocity = GlobalTransform.Basis * new Vector3(0, 0, -velocity);
        RotateY(Mathf.DegToRad(90));
        Translate(new Vector3(0, 0, -1));
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
        lifeTime -= (float)delta;
        if (lifeTime < 0) QueueFree();
    }
}

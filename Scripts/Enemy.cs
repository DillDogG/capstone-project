using Godot;
using System;

public partial class Enemy : CharacterBody3D, Damageable
{
    [Export]
    public int Speed { get; set; } = 20;

    [Export]
    public double MaxHealth = 150;

    [Export]
    public Player Player { get; set; } = null;

    [Export]
    public NavigationAgent3D NavAgent { get; set; }

    //[Export]
    //public double InvincibilityDuration = 2;

    private double Health { get; set; }

    [Export]
    public double AttackRange { get; set; }

    [Export]
    public Weapon Weapon { get; set; }

    [Export]
    public HitMarker HitMarker { get; set; } = null;

    [Export]
    public Timer SpawnTimer { get; set; }

    //private double InvincibilityTime { get; set; }

    //public virtual Area3D MovementZone { get; set; }

    public override void _Ready()
    {
        Health = MaxHealth;
        Weapon.Equip();
    }

    public void Initialize(Vector3 startPosition, Player player, HitMarker hit)
    {
        if (Player == null) Player = player;
        if (HitMarker == null) HitMarker = hit;
        Position = startPosition;
    }

    public void FullEnemyUpdate(double delta)
    {
        // was from when invincibility happened when getting hit
        //if (InvincibilityTime > 0) InvincibilityTime -= delta;
        if (Weapon != null) Weapon.MainUpdate(delta);
    }

    public override void _PhysicsProcess(double delta)
    {
        FullEnemyUpdate(delta);
        var direction = Vector3.Zero;
        NavAgent.TargetPosition = Player.GlobalTransform.Origin;
        var nextNavPoint = NavAgent.GetNextPathPosition();
        direction = (nextNavPoint - GlobalTransform.Origin).Normalized();
        Velocity = direction * Speed;
        LookAt(new Vector3(Player.GlobalPosition.X, 0, Player.GlobalPosition.Z));
        if (InAttackRange())
        {
            Weapon.Attack();
        }
        MoveAndSlide();
    }

    public bool InAttackRange()
    {
        return GlobalPosition.DistanceTo(Player.GlobalPosition) <= AttackRange;
    }

    public void ApplyDamage(double damage)
    {
        //if (InvincibilityTime > 0) return;
        Health -= damage;
        //InvincibilityTime = InvincibilityDuration;
        if (Health <= 0)
        {
            HitMarker.MainUpdate(damage, true);
            // kill method
            QueueFree();
        }
        else
        {
            HitMarker.MainUpdate(damage);
        }
    }
}

using Godot;
using System;

public partial class Enemy : CharacterBody3D, Damageable
{
    [Export]
    public int Speed { get; set; } = 20;
    private int BaseSpeed;

    [Export]
    public double MaxHealth = 100;

    [Export]
    public Player Player { get; set; } = null;

    [Export]
    public NavigationAgent3D NavAgent { get; set; }

    private double Health { get; set; }

    [Export]
    public double AttackRange { get; set; }

    [Export]
    public MeleeWeapon Weapon { get; set; }

    [Export]
    public HitMarker HitMarker { get; set; } = null;


    [Export]
    AnimationPlayer animation;

    [Export]
    public int CredValue { get; set; }

    private bool Dead = false;

    public override void _Ready()
    {
        Health = MaxHealth;
        Weapon.Equip();
        BaseSpeed = Speed;
    }

    public void Initialize(Vector3 startPosition, Player player, HitMarker hit)
    {
        if (Player == null) Player = player;
        if (HitMarker == null) HitMarker = hit;
        Position = startPosition;
    }

    public void FullEnemyUpdate(double delta)
    {
        if (Weapon != null) Weapon.MainUpdate(delta);
        if (Weapon.AttackDuration <= 0 && !Dead) Speed = BaseSpeed;
        if (!animation.IsPlaying() && Dead) QueueFree();
    }

    public override void _PhysicsProcess(double delta)
    {
        FullEnemyUpdate(delta);
        var direction = Vector3.Zero;
        NavAgent.TargetPosition = Player.GlobalTransform.Origin;
        var nextNavPoint = NavAgent.GetNextPathPosition();
        direction = (nextNavPoint - GlobalTransform.Origin).Normalized();
        Velocity = direction * Speed;
        if (!Dead) { LookAt(new Vector3(Player.GlobalPosition.X, Player.GlobalPosition.Y, Player.GlobalPosition.Z)); Rotation = new Vector3(Math.Clamp(Rotation.X, -0.8f, 0.5f), Rotation.Y, Rotation.Z); }
        if (InAttackRange() && !Dead)
        {
            animation.Play("Zombie/ZombieAttack");
            Weapon.Attack();
            Speed = 0;
        }
        if (!animation.IsPlaying()) animation.Play("Zombie/ZombieRun");
        MoveAndSlide();
    }

    public bool InAttackRange()
    {
        Vector3 EnemyPosition = GlobalPosition;
        EnemyPosition.Y = 0;
        Vector3 PlayerPosition = Player.GlobalPosition;
        PlayerPosition.Y = 0;
        return EnemyPosition.DistanceTo(PlayerPosition) <= AttackRange;
    }

    public void ApplyDamage(double damage)
    {
        if (Dead) return;
        Health -= damage;
        if (Health <= 0)
        {
            HitMarker.MainUpdate(damage, true);
            Dead = true;
            Speed = 0;
            animation.Play("Zombie/ZombieDying");
            Player.AddCredits(CredValue);
        }
        else
        {
            HitMarker.MainUpdate(damage);
        }
    }
}

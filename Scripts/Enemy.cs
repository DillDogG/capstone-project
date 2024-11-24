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

    //[Export]
    //public double InvincibilityDuration = 2;

    private double Health { get; set; }

    [Export]
    public double AttackRange { get; set; }

    [Export]
    public MeleeWeapon Weapon { get; set; }

    [Export]
    public HitMarker HitMarker { get; set; } = null;


    [Export]
    AnimationPlayer animation;

    private bool Dead = false;

    //private double InvincibilityTime { get; set; }

    //public virtual Area3D MovementZone { get; set; }

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
        // was from when invincibility happened when getting hit
        //if (InvincibilityTime > 0) InvincibilityTime -= delta;
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
        if (!Dead) LookAt(new Vector3(Player.GlobalPosition.X, 0, Player.GlobalPosition.Z));
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
            Dead = true;
            Speed = 0;
            animation.Play("Zombie/ZombieDying");
        }
        else
        {
            HitMarker.MainUpdate(damage);
        }
    }
}

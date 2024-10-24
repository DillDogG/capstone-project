using Godot;
using System;
using System.Diagnostics;
using static Godot.TextServer;

public partial class Player : CharacterBody3D, Damageable
{
	[Export]
	public int Speed { get; set; } = 20;

	// Vertical impulse applied to the character upon jumping in meters per second.
	[Export]
	public int JumpImpulse { get; set; } = 30;

	// The downward acceleration when in the air, in meters per second squared.
	[Export]
	public int FallAcceleration { get; set; } = 75;

	[Export]
	public double MaxHealth = 150;

	[Export]
    public double CoyoteDuration = 0.25;

	[Export]
    public double InvincibilityDuration = 2;

	[Export]
	public double JumpBuffer = 0.1;

	private double Health { get; set; }

    public bool Sprinting { get; set; } = false;
    public bool Crouching { get; set; } = false;
    public bool WallRunning { get; set; } = false;
	// first bool is for if player just landed, second is used to confirm that they weren't already on the ground when testing if just landed
    public bool[] JustLanded { get; set; } = { false, true };

	private int JumpCount { get; set; }
    private double CoyoteTime { get; set; }
    private double InvincibilityTime { get; set; }
	private double JumpSaveTime { get; set; } = 0;

    private Vector3 _targetVelocity = Vector3.Zero;

	public Weapon Weapon { get; set; }

	[Export]
	public Weapon[] Inventory { get; set; }

	Node3D pivot;

    public override void _Ready()
    {
        base._Ready();
        Input.MouseMode = Input.MouseModeEnum.Captured;
        pivot = GetNode<Node3D>("Pivot");
        CoyoteTime = CoyoteDuration;
		Health = MaxHealth;
		Weapon = Inventory[0];
		Weapon.Equip();
    }

	// Function to update many items that need to be updated every frame. 'delta' is seconds since last update, which is normally a small number.
	public void FullPlayerUpdate(double delta)
	{
        // reduces weapon cooldown
        if (Weapon != null) Weapon.MainUpdate(delta);

        // reduce the duration of invincibility if player was recently hit
        if (InvincibilityTime > 0) InvincibilityTime -= delta;

        // resetting extra jumps while on the ground
        if (IsOnFloor())
        {
            JumpCount = 2;
            CoyoteTime = CoyoteDuration;
            if (!JustLanded[0] && JustLanded[1]) { JustLanded[0] = true; JustLanded[1] = false; }
            else JustLanded[0] = false;
        }
        else if (CoyoteTime > 0) CoyoteTime -= delta;
        else
        {
            JustLanded[0] = false;
            JustLanded[1] = true;
        }

        // used to create a jump buffer if you press the button early
        if (JumpSaveTime > 0) JumpSaveTime -= delta;

        if (IsOnWallOnly()) WallRunning = true;
        else WallRunning = false;
    }

	public override void _PhysicsProcess(double delta)
	{
		MoveAndSlide();
		// We create a local variable to store the input direction.
		var direction = Vector3.Zero;

		// separate function for the total update
		FullPlayerUpdate(delta);

        // take in input turn that into velocity
        if (Input.IsActionPressed("move_right")) direction.Z += 1.0f;
        if (Input.IsActionPressed("move_left")) direction.Z -= 1.0f;
        if (Input.IsActionPressed("move_backward")) direction.X -= 1.0f;
        if (Input.IsActionPressed("move_forward")) direction.X += 1.0f;

        // normalize velocity, multiply by rotation to make it based on where you are facing, and normalize again
		if (direction != Vector3.Zero)
		{
			direction = direction.Normalized();
			direction = direction.Rotated(new Vector3(0, 1, 0), pivot.Rotation.Y);
			direction = direction.Normalized();
		}

        // checks if player is crouching or sprinting
		if (Input.IsActionPressed("sprint")) Sprinting = true;
		else Sprinting = false;
		if (Input.IsActionPressed("crouch")) Crouching = true;
		else Crouching = false;

		// multiplies velocity by speed, then saves in a variable for exterior alteration
		_targetVelocity.X = direction.X * Speed;
		_targetVelocity.Z = direction.Z * Speed;
        PlayerSpeedMult();

		// gravity
		if (!IsOnFloor()) _targetVelocity.Y -= FallAcceleration * (float)delta;

		PlayerJumpFunction();

        // if possible move this into a separate function and call from input instead of update
		if (Input.IsActionJustPressed("Melee"))
		{
            // set animation based on weapon type
			Weapon.Attack();
		}

		PlayerMovementMomentumFunction();
		Velocity = _targetVelocity;
		MoveAndSlide();
	}

    public void PlayerSpeedMult()
    {
        // makes player travel faster if sprinting, prevents sprinting while crouching
        if (Sprinting && !Crouching)
        {
            _targetVelocity.X *= 2;
            _targetVelocity.Z *= 2;
        }

        // gives a burst of movement when ending a slide
        if (Input.IsActionJustPressed("crouch") && IsOnFloor() || Crouching && JustLanded[0])
        {
            _targetVelocity.X *= 5;
            _targetVelocity.Z *= 5;
        }
    }

	public void PlayerJumpFunction()
	{
        if (CoyoteTime > 0 && (Input.IsActionJustPressed("jump") || JumpSaveTime > 0))
        {
            _targetVelocity.Y = JumpImpulse;
            CoyoteTime = 0;
        }
        else if (WallRunning && (Input.IsActionJustPressed("jump") || JumpSaveTime > 0))
        {
            JumpCount = 2;
            _targetVelocity.Y = JumpImpulse;
        }
        else if (JumpCount > 0 && Input.IsActionJustPressed("jump"))
        {
            _targetVelocity.Y = JumpImpulse;
            JumpCount--;
        }
        else if (Input.IsActionJustPressed("jump")) JumpSaveTime = JumpBuffer;

        // when releasing jump, slow down significantly to give more control when jumping
        if (Input.IsActionJustReleased("jump") && Velocity.Y > 0) _targetVelocity.Y *= 0.25f;
    }

	// used for altering player movement beyond the original movement, for sprinting sliding or keeping momentum
	public void PlayerMovementMomentumFunction()
	{
        // Moving the character if sliding / else not sliding
        if (Crouching && IsOnFloor())
        {
            _targetVelocity.X = (Math.Abs(Velocity.X) > Math.Abs(_targetVelocity.X) * 0.5f) ? Velocity.X * 0.99f : _targetVelocity.X * 0.5f;
            _targetVelocity.Z = (Math.Abs(Velocity.Z) > Math.Abs(_targetVelocity.Z) * 0.5f) ? Velocity.Z * 0.99f : _targetVelocity.Z * 0.5f;
            // enable to slow down sliding at a point
            _targetVelocity.X = (Math.Abs(_targetVelocity.X) < 2) ? 0 : _targetVelocity.X;
            _targetVelocity.Z = (Math.Abs(_targetVelocity.Z) < 2) ? 0 : _targetVelocity.Z;
			return;
        }
        else if (!IsOnFloor())
        {
            if (Velocity.X > _targetVelocity.X && _targetVelocity.X > 0 || Velocity.X < _targetVelocity.X && _targetVelocity.X < 0) _targetVelocity.X = Velocity.X;
            if (Velocity.Z > _targetVelocity.Z && _targetVelocity.Z > 0 || Velocity.Z < _targetVelocity.Z && _targetVelocity.Z < 0) _targetVelocity.Z = Velocity.Z;
			return;
        }
        //else Velocity = _targetVelocity;
    }

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion motionEvent)
		{
			Vector2 mouseMovement = motionEvent.ScreenRelative;
			pivot.Rotation = new Vector3(mouseMovement.Y * 0.01f + pivot.Rotation.X, mouseMovement.X * -0.01f + pivot.Rotation.Y, pivot.Rotation.Z);
		}

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

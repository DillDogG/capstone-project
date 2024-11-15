using Godot;
using System;
using System.Diagnostics;
using static Godot.TextServer;

// theoretical smooth fov change
// add_FOV * delta + base_FOV

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

	//[Export]
    //public double InvincibilityDuration = 2;

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
    //private double InvincibilityTime { get; set; }
	private double JumpSaveTime { get; set; } = 0;
    public float MouseSensitivity { get; set; } = 0.01f;

    private Vector3 _targetVelocity = Vector3.Zero;

	public Weapon Weapon { get; set; }
    private int WeaponSlot { get; set; }

	[Export]
	public Weapon[] Inventory { get; set; }

	Node3D pivot;
    [Export]
    public AmmoLabel ammoDisp { get; set; }
    [Export]
    public HealthBar healthDisp { get; set; }
    [Export]
    public TimerLabel GlobalTimer { get; set; }
    [Export]
    public Game Game { get; set; }
    Camera3D cam;
    public double BaseFOV = 75;

    public override void _Ready()
    {
        base._Ready();
        Input.MouseMode = Input.MouseModeEnum.Captured;
        cam = GetNode<Camera3D>("Pivot/Camera3D");
        pivot = GetNode<Node3D>("Pivot");
        CoyoteTime = CoyoteDuration;
		Health = MaxHealth;
        foreach (var item in Inventory)
        {
            if (item is RangedWeapon)
            {
                RangedWeapon gun = (RangedWeapon)item;
                gun.GlobalTimer = GlobalTimer;
                gun.HitCheck = GetNode<RayCast3D>("Pivot/Camera3D/BulletCheck");
            }
            item.Unequip();
        }
		Weapon = Inventory[0];
        WeaponSlot = 0;
		Weapon.Equip();
        healthDisp.MainUpdate(Health);
        FOV_Ending = BaseFOV;
    }

	// Function to update many items that need to be updated every update. 'delta' is seconds since last update, which is normally a small number.
	public void FullPlayerUpdate(double delta)
	{
        // reduces weapon cooldown
        if (Weapon != null) Weapon.MainUpdate(delta);

        // reduce the duration of invincibility if player was recently hit
        //if (InvincibilityTime > 0) InvincibilityTime -= delta;

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

        if (Weapon is RangedWeapon)
        {
            ammoDisp.Visible = true;
            ammoDisp.MainUpdate((RangedWeapon)Weapon);
        }
        else ammoDisp.Visible = false;
        if (cam.Fov != FOV_Ending) SmoothFOVUpdate(delta);
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
			pivot.Rotation = new Vector3(mouseMovement.Y * MouseSensitivity + pivot.Rotation.X, mouseMovement.X * -MouseSensitivity + pivot.Rotation.Y, pivot.Rotation.Z);
            pivot.Rotation = new Vector3(Math.Clamp(pivot.Rotation.X, -1.8f, 0.8f), pivot.Rotation.Y, pivot.Rotation.Z);
		}
        if (@event is InputEventMouseButton mouseEvent)
        {
            if (mouseEvent.Pressed)
            {
                switch (mouseEvent.ButtonIndex)
                {
                    case MouseButton.WheelUp:
                        if (WeaponSlot <= 0) WeaponSlot = Inventory.Length - 1;
                        else WeaponSlot--;
                        Weapon.Unequip();
                        Weapon = Inventory[WeaponSlot];
                        Weapon.Equip();
                        break;
                    case MouseButton.WheelDown:
                        if (WeaponSlot >= Inventory.Length - 1) WeaponSlot = 0;
                        else WeaponSlot++;
                        Weapon.Unequip();
                        Weapon = Inventory[WeaponSlot];
                        Weapon.Equip();
                        break;
                    case MouseButton.Right:
                        Camera3D cam = GetNode<Camera3D>("Pivot/Camera3D");
                        if (cam != null)
                        {
                            //cam.Fov *= 0.5f;
                            SmoothFOV(BaseFOV * 0.5, 0.5);
                            MouseSensitivity *= 0.5f;
                        }
                        break;
                }
            }
            else if (mouseEvent.IsActionReleased("ADS"))
            {
                if (cam != null)
                {
                    //cam.Fov *= 2;
                    SmoothFOV(BaseFOV, 0.5);
                    MouseSensitivity *= 2;
                }
            }
            if (Input.IsActionJustPressed("Shoot"))
            {
                // add animation
                Weapon.Attack();
            }
            if (Input.IsActionJustReleased("Shoot")) Weapon.EndAttack();
        }
        if (@event is InputEventKey)
        {
            if (Input.IsActionJustPressed("Reload") && Weapon is RangedWeapon)
            {
                RangedWeapon gun = (RangedWeapon)Weapon;
                gun.Reload();
            }
            if (Input.IsActionJustPressed("Pause"))
            {
                Game.Pause();
            }
        }
	}

    public void ApplyDamage(double damage)
    {
		//if (InvincibilityTime > 0) return;
        Health -= damage;
		//InvincibilityTime = InvincibilityDuration;
        healthDisp.MainUpdate(Health);
		if (Health <= 0)
		{
			// kill method
		}
    }

    // used to update the camera field of view without having it instantly snap into place
    private double FOV_Starting;
    private double FOV_Ending;
    private double FOV_Duration;
    private double FOV_TimeElapsed;
    public void SmoothFOVUpdate(double delta)
    {
        //if (cam.Fov == FOV_Ending)
        //{
        //    FOV_TimeElapsed = 0;
        //    return;
        //}
        FOV_TimeElapsed += delta;
        if (FOV_TimeElapsed >= FOV_Duration) { cam.Fov = (float)FOV_Ending; return; }
        double FOV_Change = FOV_Ending - FOV_Starting;
        double time = FOV_Duration / delta;
        cam.Fov += (float)FOV_Change / (float)time;
    }

    public void SmoothFOV(double ending, double duration)
    {
        FOV_Starting = cam.Fov;
        FOV_Ending = ending;
        FOV_Duration = duration;
        FOV_TimeElapsed = 0;
    }
}

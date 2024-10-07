using Godot;
using System;

public partial class Player : CharacterBody3D
{
	[Export]
	public int Speed { get; set; } = 14;

    // Vertical impulse applied to the character upon jumping in meters per second.
    [Export]
    public int JumpImpulse { get; set; } = 20;

    // The downward acceleration when in the air, in meters per second squared.
    [Export]
    public int FallAcceleration { get; set; } = 75;

    private Vector3 _targetVelocity = Vector3.Zero;

    //private Vector2 prevMousePos = Vector2.Zero;

    public override void _Ready()
    {
        base._Ready();
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _PhysicsProcess(double delta)
    {
        // We create a local variable to store the input direction.
        var direction = Vector3.Zero;

        // We check for each move input and update the direction accordingly.
        // original is commented out, swap if it works again
        if (Input.IsActionPressed("move_right"))
        {
            //direction.X += 1.0f;
            direction.Z += 1.0f;
        }
        if (Input.IsActionPressed("move_left"))
        {
            //direction.X -= 1.0f;
            direction.Z -= 1.0f;
        }
        if (Input.IsActionPressed("move_backward"))
        {
            // Notice how we are working with the vector's X and Z axes.
            // In 3D, the XZ plane is the ground plane.
            //direction.Z += 1.0f;
            direction.X -= 1.0f;
        }
        if (Input.IsActionPressed("move_forward"))
        {
            //direction.Z -= 1.0f;
            direction.X += 1.0f;
        }

        if (direction != Vector3.Zero)
        {
            direction = direction.Normalized();
            // Setting the basis property will affect the rotation of the node.
            //GetNode<Node3D>("Pivot").Basis = Basis.LookingAt(direction);
        }

        // Ground velocity
        _targetVelocity.X = direction.X * Speed;
        _targetVelocity.Z = direction.Z * Speed;

        // Vertical velocity
        if (!IsOnFloor()) // If in the air, fall towards the floor. Literally gravity
        {
            _targetVelocity.Y -= FallAcceleration * (float)delta;
        }

        // Jumping.
        if (IsOnFloor() && Input.IsActionJustPressed("jump"))
        {
            _targetVelocity.Y = JumpImpulse;
        }

        // Moving the character
        Velocity = _targetVelocity;
        MoveAndSlide();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion motionEvent)
        {
            Vector2 mouseMovement = motionEvent.ScreenRelative;
            //mouseMovement.Y = clamp(mouseMovement.Y, -1.5,1.2);
            GetNode<Node3D>("Pivot").RotateObjectLocal((new Vector3(0, 1, 0)), (mouseMovement.X * -0.05f));
            GetNode<Node3D>("Pivot").RotateObjectLocal((new Vector3(1, 0, 0)), (mouseMovement.Y * -0.05f));
            //GetNode<Node3D>("Pivot").RotateY(mouseMovement.X * -0.05f);
            //GetNode<Node3D>("Pivot").RotateX(mouseMovement.Y * -0.05f);
        }
    }
}

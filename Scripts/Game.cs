using Godot;
using System;

public partial class Game : Node3D
{
	[Export]
	public Control PauseMenu { get; set; }
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		PauseMenu.Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void Pause()
	{
		Input.MouseMode = Input.MouseModeEnum.Visible;
        GetTree().Paused = true;
        PauseMenu.Show();
    }

	public void UnPause()
	{
        Input.MouseMode = Input.MouseModeEnum.Captured;
        PauseMenu.Hide();
        GetTree().Paused = false;
	}
}

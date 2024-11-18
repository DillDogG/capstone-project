using Godot;
using System;

public partial class Game : Node3D
{
	[Export]
	public Control PauseMenu { get; set; }

	[Export]
	public Control SettingsMenu { get; set; }

	public ReadFile ReadFile { get; set; } = new ReadFile();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		PauseMenu.Hide();
		SettingsMenu.Hide();
		ReadFile.ConfirmFiles();
        if (SettingsMenu is Settings)
        {
            Settings menu = (Settings)SettingsMenu;
			ReadFile.LoadSettings(menu);
        }
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

	public void Settings()
	{
        PauseMenu.Hide();
		SettingsMenu.Show();
    }

	public void ExitSettings()
	{
        PauseMenu.Show();
        SettingsMenu.Hide();
    }
}

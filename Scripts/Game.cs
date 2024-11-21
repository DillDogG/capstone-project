using Godot;
using System;
using System.Collections.Generic;

public partial class Game : Node3D
{
	[Export]
	public Control PauseMenu { get; set; }

	[Export]
	public Control SettingsMenu { get; set; }

	public ReadFile ReadFile { get; set; } = new ReadFile();

    [Export]
    public PackedScene EnemyPrefab { get; set; }

    [Export]
    public Node3D[] Spawners { get; set; }
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

    private void OnSpawnTimerTimeout()
    {
        GD.Print("Timer Complete");
        // Create a new instance of the Mob scene.
        Enemy mob = EnemyPrefab.Instantiate<Enemy>();

        Vector3 playerPosition = GetNode<Player>("Player").Position;
        List<Node3D> potentialSpawners = new List<Node3D>();
        Node3D nearest = null;
        foreach (var spawner in Spawners)
        {
            if (spawner.GlobalPosition.DistanceTo(playerPosition) <= 100) potentialSpawners.Add(spawner);
            else if (nearest == null || spawner.GlobalPosition.DistanceTo(playerPosition) < nearest.GlobalPosition.DistanceTo(playerPosition)) nearest = spawner;
        }
        if (potentialSpawners.Count == 0) potentialSpawners.Add(nearest);
        var mobSpawnLocation = potentialSpawners[(int)GD.Randi() % (potentialSpawners.Count - 1)];

        //Vector3 playerPosition = GetNode<Player>("Player").Position;
        mob.Initialize(mobSpawnLocation.Position, GetNode<Player>("Player"), GetNode<HitMarker>("UserInterface/HitMarker"));

        // Spawn the mob by adding it to the Main scene.
        AddChild(mob);
    }
}

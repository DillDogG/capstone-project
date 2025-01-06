using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

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
    public EnemySpawner?[] Spawners { get; set; }

    [Export]
    public string SceneName { get; set; }

    public Player player { get; set; }

    [Export]
    public Vector3[] Checkpoints { get; set; }
    public int Checkpoint { get; set; }

    [Export]
    public DeathFade Death;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		PauseMenu.Hide();
		SettingsMenu.Hide();
		ReadFile.ConfirmFiles();
        player = GetNode<Player>("Player");
        if (SettingsMenu is Settings)
        {
            Settings menu = (Settings)SettingsMenu;
			ReadFile.LoadSettings(menu);
        }
        if (ReadFile.WasGameLoaded())
        {
            Checkpoint = ReadFile.LoadGame(player);
            player.Position = Checkpoints[Checkpoint];
        }
        else
        {
            ReadFile.LoadStats(player);
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
        SettingsMenu.Hide();
        if (SettingsMenu is Settings)
        {
            Settings menu = (Settings)SettingsMenu;
            ReadFile.LoadSettings(menu);
        }
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
        if (SettingsMenu is Settings)
        {
            Settings menu = (Settings)SettingsMenu;
            ReadFile.LoadSettings(menu);
        }
        SettingsMenu.Hide();
    }

    public void Dead()
    {
        Input.MouseMode = Input.MouseModeEnum.Visible;
        Death.FadeIn = true;
        GetTree().Paused = true;
    }

    public void SaveGame()
    {
        foreach (var Weapon in player.Inventory)
        {
            if (Weapon is SniperRifleRanged)
            {
                SniperRifleRanged Sniper = (SniperRifleRanged)Weapon;
                ReadFile.SaveGame(SceneName, 0, player.GlobalTimer._timer, player.Health, player.Credits, player.Inventory.ToArray(), Sniper.AmmoCount, Sniper.AmmoReserves);
                return;
            }
        }
        ReadFile.SaveGame(SceneName, 0, player.GlobalTimer._timer, player.Health, player.Credits, new Weapon[0], 0, 0);
    }

    private void OnSpawnTimerTimeout()
    {
        // Create a new instance of the Mob scene.
        Enemy mob = EnemyPrefab.Instantiate<Enemy>();

        Vector3 playerPosition = player.Position;
        List<Node3D> potentialSpawners = new List<Node3D>();
        Node3D nearest = null;
        foreach (EnemySpawner spawner in Spawners)
        {
            if (spawner == null) continue;
            if (spawner.GlobalPosition.DistanceTo(playerPosition) <= 100) potentialSpawners.Add(spawner);
            else if (nearest == null || spawner.GlobalPosition.DistanceTo(playerPosition) < nearest.GlobalPosition.DistanceTo(playerPosition)) nearest = spawner;
        }
        if (potentialSpawners.Count == 0 && nearest != null) potentialSpawners.Add(nearest);
        Node3D? mobSpawnLocation = null;
        Random random = new Random();
        if (potentialSpawners.Count > 0) mobSpawnLocation = potentialSpawners[random.Next(potentialSpawners.Count)];

        //Vector3 playerPosition = GetNode<Player>("Player").Position;
        if (mobSpawnLocation == null) return;
        mob.Initialize(mobSpawnLocation.Position, player, GetNode<HitMarker>("UserInterface/HitMarker"));
        // Spawn the mob by adding it to the Main scene.
        AddChild(mob);
    }
}

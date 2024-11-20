using Godot;
using System;

public partial class EnemySpawner : Node3D
{
    [Export]
    public PackedScene EnemyPrefab { get; set; }
    [Export]
    public Game Game;

    // We also specified this function name in PascalCase in the editor's connection window.
    private void OnMobTimerTimeout()
    {
        // Create a new instance of the Mob scene.
        Enemy mob = EnemyPrefab.Instantiate<Enemy>();

        // Choose a random location on the SpawnPath.
        // We store the reference to the SpawnLocation node.
        var mobSpawnLocation = GetNode<PathFollow3D>("SpawnPath/SpawnLocation");
        // And give it a random offset.
        mobSpawnLocation.ProgressRatio = GD.Randf();

        //Vector3 playerPosition = GetNode<Player>("Player").Position;
        mob.Initialize(mobSpawnLocation.Position);

        // Spawn the mob by adding it to the Main scene.
        Game.AddChild(mob);
    }
}

using Godot;
using System;

public partial class EnemySpawner : Node3D
{
	[Export]
	public Game game { get; set; }

	public int spawnerNumber { get; set; }

	[Export]
	public Area3D area { get; set; }

    // if 0 then won't reenable
    [Export]
    public double ReenableTime { get; set; } = 0;

    [Export]
    public int CheckpointEnable { get; set; } = 0;

    [Export]
    public int CheckpointDisable { get; set; } = 1;

    public override void _Ready()
    {
        for (var x = 0; x < game.Spawners.Length; x++)
        {
            if (game.Spawners[x] == this) { spawnerNumber = x; break; }
        }
    }

    public void DisabledOnLoad(int checkpoint)
    {
        if (checkpoint >= CheckpointDisable)
        {
            animationTime = 4;
        }
        else if (checkpoint < CheckpointEnable)
        {
            animationTime = 4;
        }
    }

	public void DisableSpawning()
	{
		game.Spawners[spawnerNumber] = null;
	}

	double animationTime = 0;
	public bool AnimationPlaying = false;
    int PauseTime = 0;
	//bool BottomPlankPlaced = false;
	//bool MidBottomPlankPlaced = false;
	//bool MidTopPlankPlaced = false;
	//bool TopPlankPlaced = false;

    [Export]
    AnimationPlayer animation;
    double ReenableTimer = 0;
    public override void _PhysicsProcess(double delta)
    {
        if (DoMain) MainUpdate(delta);
        if (game.Checkpoint < CheckpointDisable && game.Checkpoint >= CheckpointEnable)
        {
            if (ReenableTimer > 0)
            {
                ReenableTimer -= delta;
                if (ReenableTimer <= 0)
                {
                    DoMain = true;
                    game.Spawners[spawnerNumber] = this;
                    animationTime = 0;
                    PauseTime = 0;
                    animation.SpeedScale = 0;
                    animation.Seek(animationTime, true);
                }
            }
        }
        else
        {
            animation.Play("Closing");
            animation.SpeedScale = 0;
            animation.Seek(4, true);
            DisableSpawning();
            ReenableTimer = 1;
            DoMain = false;
        }
    }

    bool DoMain = true;
    // exists purely to be able to disable main function
	public void MainUpdate(double delta)
	{
        if (area.HasOverlappingBodies())
        {
            foreach (var Overlaps in area.GetOverlappingBodies())
            {
                if (Overlaps is Player && Input.IsActionPressed("Interact"))
                {
                    AnimationPlaying = true;
                    break;
                }
                else AnimationPlaying = false;
            }
        }
        if (AnimationPlaying && animationTime < 4)
        {
            animationTime += delta;
            animation.SpeedScale = 1;
            animation.Play("Closing");
            PauseTime = (int)Math.Round(animationTime, 0, MidpointRounding.ToZero);
        }
        else if (animationTime >= 4)
        {
            animation.Play("Closing");
            animation.SpeedScale = 0;
            animation.Seek(4, true);
            DisableSpawning();
            ReenableTimer = ReenableTime;
            DoMain = false;
        }
        else if (animationTime > PauseTime)
        {
            animation.Play("Closing");
            animationTime -= delta * 2;
            animation.SpeedScale = -2;
        }
        else
        {
            animation.Play("Closing");
            animationTime = PauseTime;
            animation.SpeedScale = 0;
            animation.Seek(animationTime, true);
        }
    }
}

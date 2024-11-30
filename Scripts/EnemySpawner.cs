using Godot;
using System;

public partial class EnemySpawner : Node3D
{
	[Export]
	public Game game { get; set; }

	[Export]
	public int spawnerNumber { get; set; }

	[Export]
	public Area3D area { get; set; }

	public void DisableSpawning()
	{
		game.Spawners[spawnerNumber] = null;
	}

	double animationTime = 0;
	public bool AnimationPlaying = false;
	//bool BottomPlankPlaced = false;
	//bool MidBottomPlankPlaced = false;
	//bool MidTopPlankPlaced = false;
	//bool TopPlankPlaced = false;

    [Export]
    AnimationPlayer animation;
    public override void _PhysicsProcess(double delta)
    {
        if (DoMain) MainUpdate(delta);
    }
    bool DoMain = true;
    // exists purely to be able to disable main function
	public void MainUpdate(double delta)
	{
        if (area.HasOverlappingBodies() && animationTime < 4)
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
        }
        else if (animationTime >= 4)
        {
            animation.SpeedScale = 0;
            animation.Seek(4, true);
            DisableSpawning();
            DoMain = false;
        }
        else
        {
            animationTime = Math.Round(animationTime, 0, MidpointRounding.ToZero);
            animation.SpeedScale = 0;
            animation.Seek(animationTime, true);
        }
    }
}

using Godot;
using System;

public partial class BlockedDoor : InteractableObject
{
    // if 0 then won't reenable
    [Export]
    public int CheckpointDisable { get; set; }

    [Export]
    public int Price { get; set; }

    [Export]
    AnimationPlayer animation;

    [Export]
    public StaticBody3D start;

    [Export]
    public StaticBody3D end;

    public override void _Ready()
	{

	}

    public void DisabledOnLoad(int checkpoint)
    {
        if (checkpoint >= CheckpointDisable && CheckpointDisable != -1)
        {
            Useable = false;
            animation.Play("Closing");
            start.ProcessMode = ProcessModeEnum.Disabled;
            end.ProcessMode = ProcessModeEnum.Inherit;
        }
    }

    public override string DisplayText()
    {
        return "Would you like to open this door? (" + Price + " credits, Press E)";
    }

    public override void Interacted()
    {
        if (player != null && player.Credits >= Price)
        {
            Useable = false;
            animation.Play("Closing");
            start.ProcessMode = ProcessModeEnum.Disabled;
            end.ProcessMode = ProcessModeEnum.Inherit;
            player.AddCredits(-Price);
        }
    }
    //private bool Open = false;
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (game.Checkpoint >= CheckpointDisable && CheckpointDisable != -1 && Useable)
        {
            Useable = false;
            animation.Play("Closing");
            start.ProcessMode = ProcessModeEnum.Disabled;
            end.ProcessMode = ProcessModeEnum.Inherit;
        }
        //if (!animation.IsPlaying() && !Useable && !Open)
        //{
        //    animation.Play("DoorAction_001");
        //    Open = true;
        //}
    }
}

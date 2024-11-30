using Godot;
using System;

public partial class ExitDoor : Area3D
{
    [Export]
    public string TargetScene { get; set; }
    public override void _PhysicsProcess(double delta)
    {
        if (HasOverlappingBodies())
        {
            foreach (var Overlaps in GetOverlappingBodies())
            {
                if (Overlaps is Player)
                {
                    GetTree().ChangeSceneToFile("res://Scenes/" + TargetScene + ".tscn");
                    return;
                }
            }
        }
    }
}

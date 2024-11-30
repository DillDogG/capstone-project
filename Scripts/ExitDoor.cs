using Godot;
using System;

public partial class ExitDoor : Area3D
{
    [Export]
    public PackedScene TargetScene { get; set; }
    public override void _PhysicsProcess(double delta)
    {
        if (HasOverlappingBodies())
        {
            foreach (var Overlaps in GetOverlappingBodies())
            {
                if (Overlaps is Player)
                {
                    GetTree().ChangeSceneToPacked(TargetScene);
                    return;
                }
            }
        }
    }
}

using Godot;
using System;

public partial class ExitDoor : Area3D
{
    [Export]
    public string TargetScene { get; set; }

    [Export]
    public bool needsInteraction { get; set; } = false;

    [Export]
    public Label BuyDisplay { get; set; }
    public override void _PhysicsProcess(double delta)
    {
        if (HasOverlappingBodies())
        {
            foreach (var Overlaps in GetOverlappingBodies())
            {
                if (Overlaps is Player)
                {
                    if (!needsInteraction)
                    {
                        Game game = GetTree().Root.GetNode<Game>("Node3D");
                        if (game != null)
                        {
                            game.SaveGame();
                        }
                        GetTree().ChangeSceneToFile("res://Scenes/" + TargetScene + ".tscn");
                        return;
                    } else if (Input.IsActionJustPressed("Interact"))
                    {
                        Game game = GetTree().Root.GetNode<Game>("Node3D");
                        if (game != null)
                        {
                            game.SaveGame();
                        }
                        GetTree().ChangeSceneToFile("res://Scenes/" + TargetScene + ".tscn");
                        return;
                    } else
                    {
                        BuyDisplay.Text = "Would you like to travel to " + TargetScene + "? Warning, \n this is an unfinished endless map. It is recommended you stay here.";
                        return;
                    }
                }
                if (BuyDisplay.Text == "Would you like to travel to " + TargetScene + "? Warning, \n this is an unfinished endless map. It is recommended you stay here.") BuyDisplay.Text = "";
            }
        }
    }
}

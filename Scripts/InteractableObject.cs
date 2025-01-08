using Godot;
using System;

public partial class InteractableObject : Node3D
{

    [Export]
    public Area3D area { get; set; }

    [Export]
    public Label TextDisplay { get; set; }

    public bool Useable { get; set; } = true;

    [Export]
    public Game game { get; set; }

    public override void _PhysicsProcess(double delta)
    {
        if (area.HasOverlappingBodies())
        {
            foreach (var Overlaps in area.GetOverlappingBodies())
            {
                if (Overlaps is Player)
                {
                    Player player = (Player)Overlaps;

                    if (Useable) TextDisplay.Text = DisplayText();
                    else { TextDisplay.Text = ""; return; }
                    if (Input.IsActionJustPressed("Interact"))
                    {
                        Interacted();
                    }
                    break;
                }
                if (TextDisplay.Text == DisplayText()) TextDisplay.Text = "";
            }
        }
    }

    public virtual string DisplayText()
    {
        return "";
    }

    public virtual void Interacted()
    {
        Useable = false;
    }
}

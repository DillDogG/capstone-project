using Godot;
using System;

public partial class SavePoint : InteractableObject
{
    [Export]
    public int Checkpoint { get; set; }

    public override void _Ready()
	{
	}

    public override string DisplayText()
    {
        return "Would you like to save the game here? (press E)";
    }

    public override void Interacted()
    {
        game.SaveGame(Checkpoint);
    }
}

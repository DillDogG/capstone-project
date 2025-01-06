using Godot;
using System;

public partial class PauseContinue : Button
{
    public Game Game;
    private bool JustPaused = true;

    public override void _Ready()
    {
        Game = GetTree().Root.GetNode<Game>("Node3D");
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("Pause") && !JustPaused)
        {
            JustPaused = true;
            Game.UnPause();
        }
        else if (Input.IsActionJustPressed("Pause")) JustPaused = false;
    }

    public override void _Pressed()
    {
        base._Pressed();
        JustPaused = true;
        Game.UnPause();
    }
}

using Godot;
using System;

public partial class ReloadGameButton : Button
{
    public Game game;

    public override void _Ready()
    {
        game = GetTree().Root.GetNode<Game>("Node3D");
    }

    public override void _Pressed()
    {
        base._Pressed();
        game.LoadGame();
    }
}

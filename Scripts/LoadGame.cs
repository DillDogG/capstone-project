using Godot;
using System;

public partial class LoadGame : Button
{
    [Export]
    public StartScreen Game;
    public override void _Pressed()
    {
        base._Pressed();
        Game.LoadGame();
    }
}

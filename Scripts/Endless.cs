using Godot;
using System;

public partial class Endless : Button
{
    [Export]
    public StartScreen game { get; set; }
    public override void _Pressed()
    {
        base._Pressed();
        game.Endless();
    }
}

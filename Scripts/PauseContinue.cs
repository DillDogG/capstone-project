using Godot;
using System;

public partial class PauseContinue : Button
{
    [Export]
    public Game Game;
    public override void _Pressed()
    {
        base._Pressed();
        Game.UnPause();
    }
}

using Godot;
using System;

public partial class SettingsButtonStart : Button
{
    [Export]
    public StartScreen Game;

    public override void _Pressed()
    {
        GD.Print("Settings pressed");
        base._Pressed();
        Game.Settings();
    }
}

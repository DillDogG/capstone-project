using Godot;
using System;

public partial class ResetSettings : Button
{
    [Export]
    public Settings Settings;


    public override void _Pressed()
    {
        base._Pressed();
        Settings.ResetSettings();
    }
}

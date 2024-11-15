using Godot;
using System;

public partial class SaveSettings : Button
{
    [Export]
    public Settings Settings;

    public override void _Pressed()
    {
        base._Pressed();
        Settings.SaveSettings();
    }
}

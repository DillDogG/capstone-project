using Godot;
using System;

public partial class SaveSettingsStart : Button
{
    [Export]
    public StartSettings Settings;

    public override void _Pressed()
    {
        base._Pressed();
        Settings.SaveSettings();
    }
}

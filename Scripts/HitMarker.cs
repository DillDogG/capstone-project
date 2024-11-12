using Godot;
using System;

public partial class HitMarker : Label
{
    double Duration = 0;
    double TotalDamage = 0;
    bool KillString = false;
    float Opacity;

    public override void _Process(double delta)
    {
        if (Duration <= 0)
        {
            Visible = false;
            TotalDamage = 0;
            RemoveThemeColorOverride("font_color");
            KillString = false;
        }
        else
        {
            Duration -= delta;
            Visible = true;
            Opacity = (float)Duration / 2;
            if (KillString) SetModulate(new Color(1, 0.1f, 0, Opacity));
            else SetModulate(new Color(1, 1, 0, Opacity));

        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public void MainUpdate(double Damage, bool killed = false)
    {
        TotalDamage += Damage;
        Text = TotalDamage.ToString();
        Duration = 2;
        if (killed)
        {
            KillString = true;
            AddThemeColorOverride("font_color", new Color(1, 0.1f, 0));
        }
    }
}

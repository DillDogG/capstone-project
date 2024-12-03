using Godot;
using System;

public partial class DeathFade : ColorRect
{
    public bool FadeIn { get; set; } = false;

    double Duration = 0;
    float Opacity;

    public override void _PhysicsProcess(double delta)
    {
        if (FadeIn)
        {
            Duration += delta;
            Visible = true;
            Opacity = (float)Duration * 0.5f;
            SetModulate(new Color(0, 0, 0, Opacity));
        }
        if (Duration >= 4)
        {
            FadeIn = false;
            GetTree().ChangeSceneToFile("res://Scenes/DeathScene.tscn");
        }
    }
}

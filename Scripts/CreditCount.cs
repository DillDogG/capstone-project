using Godot;
using System;

public partial class CreditCount : Label
{
    public int CredCount = 0;
    public int CredsAdded = 0;
    double PauseDuration = 0;

    public void SetScore(int score)
    {
        CredCount = score;
        CredsAdded = 0;
    }
    public void MainUpdate(int NewValue)
    {
        CredsAdded += NewValue;
        PauseDuration = 1;
        if (CredsAdded > 0) Text = $"  {CredCount}\n+ {CredsAdded}";
        else if (CredsAdded < 0) Text = $"  {CredCount}\n {CredsAdded}";
    }

    public override void _PhysicsProcess(double delta)
    {
        if (CredsAdded > 1000 && PauseDuration <= 0)
        {
            Text = $"  {CredCount}\n+ {CredsAdded}";
            CredsAdded -= 100;
            CredCount += 100;
        }
        else if (CredsAdded > 100 && PauseDuration <= 0)
        {
            Text = $"  {CredCount}\n+ {CredsAdded}";
            CredsAdded -= 10;
            CredCount += 10;
        }
        else if (CredsAdded > 20 && PauseDuration <= 0)
        {
            Text = $"  {CredCount}\n+ {CredsAdded}";
            CredsAdded -= 5;
            CredCount += 5;
        }
        else if (CredsAdded > 0 && PauseDuration <= 0)
        {
            Text = $"  {CredCount}\n+ {CredsAdded}";
            CredsAdded--;
            CredCount++;
        }
        else if (CredsAdded < -1000 && PauseDuration <= 0)
        {
            Text = $"  {CredCount}\n {CredsAdded}";
            CredsAdded += 100;
            CredCount -= 100;
        }
        else if (CredsAdded < -100 && PauseDuration <= 0)
        {
            Text = $"  {CredCount}\n {CredsAdded}";
            CredsAdded += 10;
            CredCount -= 10;
        }
        else if (CredsAdded < -20 && PauseDuration <= 0)
        {
            Text = $"  {CredCount}\n {CredsAdded}";
            CredsAdded += 5;
            CredCount -= 5;
        }
        else if (CredsAdded < 0 && PauseDuration <= 0)
        {
            Text = $"  {CredCount}\n {CredsAdded}";
            CredsAdded++;
            CredCount--;
        }
        else if (PauseDuration > 0) PauseDuration -= delta;
        else Text = $"  {CredCount}";
    }
}

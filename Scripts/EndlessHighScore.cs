using Godot;
using System;

public partial class EndlessHighScore : Label
{
    [Export]
    public TimerLabel timer;
    public double bestTime = 0;
    [Export]
    public Game game;

    public override void _Process(double delta)
    {
        if (timer._timer > bestTime)
        {
            bestTime = timer._timer;
            game.SaveBestTime(bestTime);
        }
        Text = TimerFormat(bestTime);
    }

    public string TimerFormat(double time)
    {
        time = Math.Round(time, 2);
        if (time <= 60) return time.ToString();
        int minutes = (int)time / 60;
        time %= 60;
        time = Math.Round(time, 2);
        if (minutes <= 60) return $"{minutes}:{time}";

        int hours = (int)minutes / 60;
        minutes %= 60;
        return $"Best Time: {hours}:{minutes}:{time}";
    }
}

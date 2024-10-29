using Godot;
using System;

public partial class TimerLabel : Label
{
	private double _timer = 0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_timer = 0;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_timer += delta;
		Text = TimerFormat(_timer);
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
		return $"{hours}:{minutes}:{time}";
	}
}

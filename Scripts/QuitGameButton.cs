using Godot;
using System;

public partial class QuitGameButton : Button
{
    public override void _Pressed()
	{
        GetTree().Root.PropagateNotification((int)NotificationWMCloseRequest);
		GetTree().Quit();
    }
}

using Godot;
using System;

public partial class TankTrail : Sprite2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var tw =CreateTween();
		tw.TweenInterval(1);
		tw.TweenProperty(this, "modulate", new Color(1, 1, 1, 0),0.5);
		tw.TweenCallback(Callable.From(QueueFree));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

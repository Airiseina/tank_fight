using Godot;
using System;

public partial class PlayerBullet : Area2D
{
	[Export] public float Speed {get; set;} = 100f;
	private VisibleOnScreenNotifier2D visibleOnScreenNotifier2D;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		visibleOnScreenNotifier2D = GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier");
		AreaEntered += OnAreaEntered;
		visibleOnScreenNotifier2D.ScreenExited += OnScreenExited;
	}
	private void OnAreaEntered(Area2D area)
	{
		if (area is Enemy enemy)
		{
			enemy.ReduceHealth();
			GD.Print("hit enemy");
			QueueFree();
		}
	}
	private void OnScreenExited()
	{
		QueueFree();
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Position+= Transform.X * Speed * (float)delta;
	}
}

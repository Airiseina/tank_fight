using Godot;
using System;

public partial class TrailComponent : Node2D
{
	[Export] public PackedScene TrailScene { get; set; }
	private Timer timer;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		timer = GetNode<Timer>("Timer");
		timer.Timeout += generateTrail;
	}
	public void Start()
	{
		if (timer.IsStopped())
		{
			timer.Start();
		}
	}
	public void Stop()
	{
		if (!timer.IsStopped())
		{
			timer.Stop();
		}
	}
	private void generateTrail()
	{
		var trail = TrailScene.Instantiate()as Node2D;
		if (trail == null)
		{
			return;
		}
		var owner = Owner as Node2D;
		if (owner == null)
		{
			return;
		}
		trail.GlobalPosition = owner.GlobalPosition;
		if (owner == null)
		{
			return;
		}
		trail.GlobalPosition = owner.GlobalPosition;
		trail.Rotation = owner.Rotation;
		trail.TopLevel = true;
		AddChild(trail); 
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

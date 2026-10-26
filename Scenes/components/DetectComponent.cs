using Godot;
using System;

public partial class DetectComponent : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}
	public Vector2? GetPlayerPos()
	{
		if (HasOverlappingAreas())
		{
		var targets= GetOverlappingAreas();
			if (targets.Count>0)
			{
				return targets[0].GlobalPosition; 
			}
		}
		return null;
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

using Godot;
using System;

public partial class Bullet : Node2D
{
	[Export] public float Speed { get; set; } = 400f;
	private VisibleOnScreenNotifier2D visibleOnScreenNotifier2D;
	private HitBoxComponent hitBoxComponent;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		visibleOnScreenNotifier2D = GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D");
		hitBoxComponent = GetNode<HitBoxComponent>("HitBoxComponent");
		visibleOnScreenNotifier2D.ScreenExited += onScreenExited;
		hitBoxComponent.Hit += onHit;
		var tw = CreateTween();
		tw.SetLoops();
		tw.TweenProperty(this,"modulate", new Color("ff6e00"), 0.2);
		tw.TweenProperty(this,"modulate", new Color("ffffff"), 0.2);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Position += Transform.X * Speed * (float)delta;
	}
	private void onScreenExited()
	{
		QueueFree();
	}
	private void onHit()
	{
		var gm = GetNode<GameManager>("/root/GameManager");
gm.EmitSignal(GameManager.SignalName.BulleytHit, GlobalPosition);
		QueueFree();
	}
}

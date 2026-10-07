using Godot;
using System;

public partial class VfxManager : Node2D
{
	[Export] public PackedScene expAnimScene{get;set;}
	private AudioStreamPlayer audioStreamPlayer;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		audioStreamPlayer= GetNode<AudioStreamPlayer>("AudioStreamPlayer");
		var gm = GetNode<GameManager>("/root/GameManager");
		gm.EnemyKilled += explode;
	}
	private async void explode(Vector2 pos)
	{
		var expAnim = expAnimScene.Instantiate()as AnimatedSprite2D;
		if (expAnim == null)
		{
			GD.Print("expAnim is null");
			return;
		}
		expAnim.GlobalPosition=pos;
		AddChild(expAnim);
		expAnim.Play("explosion");
		audioStreamPlayer.Play();
		await ToSignal(expAnim, AnimatedSprite2D.SignalName.AnimationFinished);
		expAnim.QueueFree();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

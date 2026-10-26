using Godot;
using System;

public partial class WeaponComponent : Node2D
{
	private bool canShoot=true;
	private float minCoolDown = 0.2f;
	[Export]public float CoolDown { get; set; } = 0.5f;
	[Export]public PackedScene BulletScene { get; set; }
	private Marker2D marker2D;
	private AudioStreamPlayer2D audioStreamPlayer2D;
	private Timer timer;
	private AnimationPlayer animationPlayer;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		marker2D = GetNode<Marker2D>("Gun/Marker2D");
		audioStreamPlayer2D = GetNode<AudioStreamPlayer2D>("ShootSound");
		timer = GetNode<Timer>("Timer");
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		timer.Timeout += OnTimeOut;
	}
	public void Target(Vector2 targetPos)
	{
		LookAt(targetPos);
	}
	private void OnTimeOut()
	{
		canShoot = true;
	}
	public void Shoot(Vector2 targetPos)
	{
		if (canShoot)
		{
			timer.Start(CoolDown);
			if (CoolDown < 0.5f)
			{
				animationPlayer.SpeedScale = 0.5f / CoolDown;
			}
			animationPlayer.Play("shoot");
			canShoot = false;
			var bullet = BulletScene.Instantiate() as Node2D;
			if (bullet == null)
			{
				return;
			}
			bullet.GlobalPosition = marker2D.GlobalPosition;
			bullet.Rotation = marker2D.GlobalRotation;
			bullet.LookAt(targetPos);
			bullet.TopLevel = true;
			AddChild(bullet);
		}
	}
	public void Upgrade(float value)
	{
		CoolDown= Math.Max(minCoolDown, CoolDown-value);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

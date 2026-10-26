using Godot;
using System;

public partial class EnemyTank : PathFollow2D
{
	[Export] public float Speed { get; set; } = 100f;
	private HurtBoxComponent hurtBoxComponent;
	private HealthComponent healthComponent;
	private AnimationPlayer animationPlayer;
	private TrailComponent trailComponent;
	private DetectComponent detectComponent;
	private WeaponComponent weaponComponent;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		hurtBoxComponent = GetNode<HurtBoxComponent>("HurtBoxComponent");
		healthComponent = GetNode<HealthComponent>("HealthComponent");
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		trailComponent = GetNode<TrailComponent>("TrailComponent");
		detectComponent = GetNode<DetectComponent>("DetectComponent");
		weaponComponent = GetNode<WeaponComponent>("WeaponComponent");
		hurtBoxComponent.GetDamage += healthComponent.GetDamage;
		healthComponent.Died += onDied;
		hurtBoxComponent.GetDamage += onGetDamage;
	}
	private void onDied()
	{
		var gm = GetNode<GameManager>("/root/GameManager");
		gm.EmitSignal(GameManager.SignalName.EntityDied, GlobalPosition, GetGroups());
		QueueFree();
	}
	private void onGetDamage(float damage)
	{
		animationPlayer.Play("flash");


	}
	private void findPlayer()
{
    Vector2? playerPos = detectComponent.GetPlayerPos();
    if (playerPos == null) return;

    Vector2 targetPos = playerPos.Value;
    weaponComponent.Target(targetPos);
    weaponComponent.Shoot(targetPos);
}
	 
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Progress += Speed * (float)delta;
		trailComponent.Start();
		findPlayer();
	}
}

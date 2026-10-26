using Godot;
using System;

public partial class EnemyTower : StaticBody2D
{
	private DetectComponent detectComponent;
	private WeaponComponent weaponComponent;
	private HurtBoxComponent hurtBoxComponent;
	private HealthComponent healthComponent;
	private AnimationPlayer animationPlayer;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		detectComponent = GetNode<DetectComponent>("DetectComponent");
		weaponComponent = GetNode<WeaponComponent>("WeaponComponent");
		hurtBoxComponent = GetNode<HurtBoxComponent>("HurtBoxComponent");
		healthComponent = GetNode<HealthComponent>("HealthComponent");
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		hurtBoxComponent.GetDamage += healthComponent.GetDamage;
		healthComponent.Died += onDied;
		hurtBoxComponent.GetDamage+= onGetDamage;

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
		var playerPos=detectComponent.GetPlayerPos();
		if (playerPos != null)
		{
			weaponComponent.Target((Vector2)playerPos);
			weaponComponent.Shoot((Vector2)playerPos);
		}
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		findPlayer();
	}
}

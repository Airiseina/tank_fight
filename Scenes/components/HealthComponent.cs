using Godot;
using System;

public partial class HealthComponent : Node
{
	[Signal] public delegate void DiedEventHandler();
	[Signal] public delegate void HealthChangedEventHandler(float healthPercent);
	[Export] public float MaxHealth { get; set; } = 5;
	private float currentHealth;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		currentHealth = MaxHealth;
	}
	public float GetHealthPercent()
	{
		if (currentHealth < 0)
		{
			return 0;
		}
		return Math.Min(currentHealth / MaxHealth, 1);
	}
	public void GetDamage(float damage)
	{
		currentHealth = Math.Max(currentHealth - damage, 0);
		EmitSignal(SignalName.HealthChanged, GetHealthPercent());
		checkDeath();
	}
	private void checkDeath()
	{
		if (currentHealth <= 0)
		{
			EmitSignal(SignalName.Died);
		}
	}
	public void Upgrade(float value)
	{
		currentHealth = Math.Min(MaxHealth, currentHealth + value);
		EmitSignal(SignalName.HealthChanged, GetHealthPercent());
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

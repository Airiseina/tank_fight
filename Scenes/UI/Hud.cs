using Godot;
using System;

public partial class Hud : Control
{
	private Label label;
	private ProgressBar progressBar;
	private Timer timer;
	private PanelContainer panelContainer3;
	private Label result;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		label = GetNode<Label>("MarginContainer/HBoxContainer/PanelContainer/Label");
        progressBar = GetNode<ProgressBar>("MarginContainer/HBoxContainer/PanelContainer2/HBoxContainer/ProgressBar");
        timer = GetNode<Timer>("Timer");
        panelContainer3 = GetNode<PanelContainer>("CenterContainer/PanelContainer3");
        result = GetNode<Label>("CenterContainer/PanelContainer3/Label");
		var gm = GetNode<GameManager>("/root/GameManager");
		gm.EnemyKilled += onEnemyKilled;
        gm.UpdateHealthUI += onUpdateHealthUI;
        gm.PlayerKilled += onPlayerKilled;
        gm.PlayerWin += onPlayerWin;
	}
	private void onEnemyKilled(Vector2 pos)
	{
		var gm = GetNode<GameManager>("/root/GameManager");
		label.Text = $"Killed: {gm.Score}";
	}
	private void onUpdateHealthUI(int health)
	{
		var value =100*health/10;
		progressBar.Value = value;
	}
	private async void onPlayerKilled()
	{
		timer.Start();
		await ToSignal(timer,Timer.SignalName.Timeout);
		result.Text = "You Lose!";
		panelContainer3.Show();
	}
	private async void onPlayerWin()
	{
		timer.Start();
		await ToSignal(timer,Timer.SignalName.Timeout);
		result.Text = "You Win!";
		panelContainer3.Show();
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

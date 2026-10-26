using Godot;
using System;

public partial class Hud : Control
{
	private Label label;
	private ProgressBar progressBar;
	private Timer timer;
	private PanelContainer panelContainer3;
	private Label result;
	private Button button;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		label = GetNode<Label>("MarginContainer/HBoxContainer/PanelContainer/Label");
        progressBar = GetNode<ProgressBar>("MarginContainer/HBoxContainer/PanelContainer2/HBoxContainer/ProgressBar");
        timer = GetNode<Timer>("Timer");
        panelContainer3 = GetNode<PanelContainer>("MarginContainer/PanelContainer3");
		panelContainer3.Hide();
        result = GetNode<Label>("MarginContainer/PanelContainer3/VBoxContainer/Label");
		button = GetNode<Button>("MarginContainer/PanelContainer3/VBoxContainer/Button");
		var gm = GetNode<GameManager>("/root/GameManager");
		gm.UpdateScoreUI += onUpdateScoreUI;
		gm.UpdateHealthUI += onUpdateHealthUI;
		gm.PlayerKilled += onPlayerKilled;
		gm.PlayerWin += onPlayerWin;
		button.Pressed += gm.Restart;
		onUpdateScoreUI(gm.Score, gm.TotalEnemySize);
    }
	private void onUpdateScoreUI(int score, int total)
	{
		label.Text = $"Killed: {score}/{total}";
	}
	private void onUpdateHealthUI(int health)
	{
		progressBar.Value = health;
	}
	private async void onPlayerKilled()
	{
		timer.Start();
		await ToSignal(timer, Timer.SignalName.Timeout);
		result.Text = "You Lose!";
		panelContainer3.Show();
	}
	private async void onPlayerWin()
	{
		timer.Start();
		await ToSignal(timer, Timer.SignalName.Timeout);
		result.Text = "You Win!";
		panelContainer3.Show();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

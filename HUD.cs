using Godot;
using System;

public class HUD : CanvasLayer
{
    [Signal] public delegate void StartGame();

    private Label messageLabel;
    private Label scoreLabel;

    private Button startButton;

    private Timer messageTimer;

    public override void _Ready()
    {
        scoreLabel = GetNode("ScoreLabel") as Label;    
        startButton = GetNode("StartButton") as Button;
        messageLabel = GetNode("MessageLabel") as Label;    
        messageTimer = GetNode("MessageTimer") as Timer;    
    }

    public void show_message(string text)
    {
        messageLabel.Text = text;
        messageLabel.Show();
        messageTimer.Start();
    }

    public void show_game_over()
    {
        messageLabel.Text = "Game Over!";
        startButton.Text = "Restart";
        startButton.Show();
        // messageLabel.Text = "Dodge the\nCreeps!";
        messageLabel.Show();
    }

    public void update_score(int score)
    {
        scoreLabel.Text = score.ToString();
    }

    public void _on_StartButton_pressed()
    {
        startButton.Hide();
        EmitSignal("StartGame");
    }

    public void _on_MessageTimer_timeout()
    {
        messageLabel.Hide();
    }
}

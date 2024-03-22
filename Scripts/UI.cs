using Godot;
using System;

public partial class UI : CanvasLayer
{

    [Signal]
    public delegate void IncreaseDifficultyEventHandler();

    [Export]
    private int _scoreToIncreaseDifficulty = 50;

    [Export]
    private float _pointUpdateInterval = 0.3f;

    private float _pointUpdateTimer = 0.0f;

    private Label _scoreLabel;
    private int _score;

    private Player _player;

    private TextureButton _restartButton;

    private VBoxContainer _gameOverContainer;

    public override void _Ready()
    {
        _gameOverContainer = GetNode<VBoxContainer>("%GameOverContainer");
        _restartButton = GetNode<TextureButton>("%RestartButton");
        _player = GetNode<Player>("/root/main/Player");

        _scoreLabel = GetNode<Label>("%ScoreLabel");
        _score = 0;
        _scoreLabel.Text = $"{_score.ToString("D5")}";

        _player.ObstacleHit += () =>
        {
            _gameOverContainer.Visible = true;
            SetProcess(false);
        };

        _restartButton.Pressed += () => GetTree().ReloadCurrentScene();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        _pointUpdateTimer += (float)delta;
        if (_pointUpdateTimer >= _pointUpdateInterval)
        {
            _score++;
            _scoreLabel.Text = $"{_score.ToString("D5")}";
            _pointUpdateTimer = 0.0f;

            if (_score % _scoreToIncreaseDifficulty == 0)
            {
                EmitSignal(SignalName.IncreaseDifficulty);
            }

        }


    }
}

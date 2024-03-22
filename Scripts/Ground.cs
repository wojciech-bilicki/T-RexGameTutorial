using Godot;
using System;

public partial class Ground : Node2D
{
    [Export]
    private float _speedIncreaseForDifficulty = -125.0f;

    [Export]
    private float _speed = -125.0f;

    private StaticBody2D _staticBody2D;
    private StaticBody2D _staticBody2D2;
    private int _textureWidth;

    private Player _player;

    private ObstacleSpawner _obstacleSpawner;


    private UI _ui;

    public override void _Ready()
    {
        _ui = GetNode<UI>("/root/main/UI");
        _player = GetNode<Player>("../Player");
        _staticBody2D = GetNode<StaticBody2D>("Ground_1");
        _staticBody2D2 = GetNode<StaticBody2D>("Ground_2");
        _textureWidth = _staticBody2D.GetNode<Sprite2D>("GroundSprite").Texture.GetWidth();
        _obstacleSpawner = GetNode<ObstacleSpawner>("ObstacleSpawner");
        _staticBody2D2.GlobalPosition = _staticBody2D2.GlobalPosition with { X = _staticBody2D.GlobalPosition.X + _textureWidth };

        _player.ObstacleHit += () =>
        {
            _speed = 0;
            _obstacleSpawner.QueueFree();
        };

        _ui.IncreaseDifficulty += () =>
        {
            _speed += _speedIncreaseForDifficulty;
            _obstacleSpawner.IncreaseDifficulty();
        };
    }

    public override void _Process(double delta)
    {
        _staticBody2D.GlobalPosition += new Vector2(_speed * (float)delta, 0);
        _staticBody2D2.GlobalPosition += new Vector2(_speed * (float)delta, 0);

        if (_staticBody2D.GlobalPosition.X < -_textureWidth)
        {
            _staticBody2D.GlobalPosition = _staticBody2D2.GlobalPosition + new Vector2(_textureWidth, 0);
        }
        if (_staticBody2D2.GlobalPosition.X < -+_textureWidth)
        {
            _staticBody2D2.GlobalPosition = _staticBody2D.GlobalPosition + new Vector2(_textureWidth, 0);
        }
    }
}

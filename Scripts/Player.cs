using Godot;
using System;

public partial class Player : CharacterBody2D
{

    [Signal]
    public delegate void ObstacleHitEventHandler();

    [Export]
    private float _jumpVelocity = -800.0f;

    [Export]
    private float _gravityMultiplier = 3.0f;

    public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();


    private RectangleShape2D _defaultShape = GD.Load<RectangleShape2D>("res://CollisionShapes/dino/dino_default.tres");
    private RectangleShape2D _crouchShape = GD.Load<RectangleShape2D>("res://CollisionShapes/dino/dino_crouching.tres");

    private AnimatedSprite2D _animatedSprite;
    private CollisionShape2D _collisionShape2D;

    private float _yOffsetCrouch = 18.0f;
    private float _collisionShapeStartYPosition;

    public override void _Ready()
    {
        _animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _collisionShape2D = GetNode<CollisionShape2D>("CollisionShape2D");
        _collisionShape2D.Shape = _defaultShape;
        _collisionShapeStartYPosition = _collisionShape2D.Position.Y;
    }

    public override void _Process(double delta)
    {

        if (IsOnFloor())
        {
            if (Input.IsActionPressed("down"))
            {
                if (_collisionShape2D.Shape != _crouchShape)
                {
                    _collisionShape2D.Shape = _crouchShape;
                    _collisionShape2D.Position = new Vector2(_collisionShape2D.Position.X, _collisionShapeStartYPosition + _yOffsetCrouch);
                    _animatedSprite.Offset = new Vector2(0, _yOffsetCrouch);
                }
                _animatedSprite.Play("crouch");
            }
            else
            {
                if (_collisionShape2D.Shape != _defaultShape)
                {
                    _collisionShape2D.Shape = _defaultShape;

                    _collisionShape2D.Position = new Vector2(_collisionShape2D.Position.X, _collisionShapeStartYPosition);
                    _animatedSprite.Offset = new Vector2(0, 0);
                }
                _animatedSprite.Play("run");
            }
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;
        if (!IsOnFloor())
        {
            velocity.Y += gravity * _gravityMultiplier * (float)delta;
        }

        if (Input.IsActionJustPressed("jump") && IsOnFloor())
        {
            velocity.Y = _jumpVelocity;
        }

        Velocity = velocity;
        var hasCollided = MoveAndSlide();

        if (!hasCollided)
        {
            return;
        }

        var moveCollider = GetLastSlideCollision().GetCollider();

        if ((moveCollider is Pterodactyl) || (moveCollider is Cactus))
        {
            SetPhysicsProcess(false);
            SetProcess(false);
            _animatedSprite.Play("dead");
            EmitSignal(SignalName.ObstacleHit);

            if (moveCollider is Pterodactyl)
            {
                (moveCollider as Pterodactyl).Stop();
            }
        }

        // Game Over


    }
}

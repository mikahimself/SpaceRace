using Godot;
using System;

public class SpaceShip : Node2D
{
    [Export]
    public int Speed = 125;
    [Export]
    public int myId = 0;
    [Signal]
    public delegate void GainPoint(int myId);
    public float autopilotSpeed = -0.5f;
    public Vector2 velocity = Vector2.Zero;
    private Vector2 _screenSize;
    private int _spriteHeight = 32;
    private int _bottomPadding = 100;
    private Vector2[] _startPositions = { new Vector2(0.40f, 0.9f), new Vector2(0.60f, 0.9f) };
    private bool autopilot = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _screenSize = GetViewport().Size;
        Position = new Vector2(_screenSize.x * _startPositions[myId - 1].x, _screenSize.y * _startPositions[myId - 1].y);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        GetControls();
        MoveShip(delta);
    }
    public void GetControls()
    {
        velocity.y = 0;

        if (Input.IsActionPressed(String.Format("up_{0}", myId))) {
            velocity.y = -1;
        }

        if (Input.IsActionPressed(String.Format("down_{0}", myId))) {
            velocity.y = 1;
        }
    }

    public void MoveShip(float delta)
    {
        int startX = (int)(_screenSize.x * _startPositions[myId -1].x);
        int startY = (int)(_screenSize.y * _startPositions[myId -1].y);

        if (autopilot)
        {
            Position += new Vector2(0, autopilotSpeed) * Speed * delta;
        } else 
        {
            Position += velocity * Speed * delta;
        }
        
        if (Position.y < - _spriteHeight / 2 && !autopilot) {
            autopilot = true;
            Position = new Vector2(startX, _screenSize.y + _bottomPadding);
            EmitSignal(nameof(GainPoint), myId);
        } else if (autopilot && Position.y <= startY) {
            autopilot = false;
            Position = new Vector2(startX, startY);
        } else if (Position.y + _spriteHeight /2 >= _screenSize.y && !autopilot) {
            Position = new Vector2(startX, _screenSize.y - _spriteHeight / 2);
        }
    }

    public void ResetShip()
    {
        Position = new Vector2(_screenSize.x * _startPositions[myId - 1].x, _screenSize.y * _startPositions[myId - 1].y);
    }

    public void _on_Area2D_area_entered(Area area)
    {
        ResetShip();
    }
}

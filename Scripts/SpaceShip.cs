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
    [Signal]
    public delegate void FirstBoost();
    public float autopilotSpeed = -0.5f;
    public Vector2 velocity = Vector2.Zero;
    private Vector2 _screenSize;
    private int _spriteHeight = 32;
    private int _bottomPadding = 100;
    private Vector2[] _startPositions = { new Vector2(0.40f, 0.9f), new Vector2(0.60f, 0.9f) };
    private bool autopilot = false;
    private bool _firstBoost = false;
    private bool _timeout = false;
    private AudioStreamPlayer2D _asp_engine;
    private AudioStreamPlayer2D _asp_effect;
      
    public override void _Ready()
    {
        GetParent().GetNode("TimerRectangle").Connect("TimeOut", this, nameof(_OnTimeOut));
        GetParent().GetNode("ScoreMinder").Connect("OnBlinkFinish", this, nameof(_OnBlinkFinished));
        _asp_engine = (AudioStreamPlayer2D)GetNode("Engine");
        _asp_effect = (AudioStreamPlayer2D)GetNode("Effect");
    }

    public override void _Process(float delta)
    {
        GetControls();
        MoveShip(delta);
    }

    public void SetupShip(Vector2 screensize)
    {
        _screenSize = screensize;
        Position = new Vector2(_screenSize.x * _startPositions[myId - 1].x, _screenSize.y * _startPositions[myId - 1].y);
    }
    public void GetControls()
    {
        velocity.y = 0;
        if (!_timeout)
        {
            if (Input.IsActionPressed(String.Format("up_{0}", myId)))
            {
                velocity.y = -1;
                _asp_engine.PitchScale += 0.0025f;
            }

            if (Input.IsActionPressed(String.Format("down_{0}", myId)))
            {
                velocity.y = 1;
                _asp_engine.PitchScale -= 0.0025f;

            }
            if (velocity.y != 0 && !_firstBoost)
            {
                _firstBoost = true;
                EmitSignal(nameof(FirstBoost));
            }
        }
    }

    public void MoveShip(float delta)
    {
        int startX = (int)(_screenSize.x * _startPositions[myId - 1].x);
        int startY = (int)(_screenSize.y * _startPositions[myId - 1].y);

        if (autopilot)
        {
            Position += new Vector2(0, autopilotSpeed) * Speed * delta;
        }
        else
        {
            Position += velocity * Speed * delta;
        }

        if (Position.y < -_spriteHeight / 2 && !autopilot)
        {
            autopilot = true;
            _asp_engine.PitchScale = 0.75f;
            Position = new Vector2(startX, _screenSize.y + _bottomPadding);
            PlayEffect("get-point");
            EmitSignal(nameof(GainPoint), myId);
        }
        else if (autopilot && Position.y <= startY)
        {
            autopilot = false;
            Position = new Vector2(startX, startY);
        }
        else if (Position.y + _spriteHeight / 2 >= _screenSize.y && !autopilot)
        {
            Position = new Vector2(startX, _screenSize.y - _spriteHeight / 2);
        }
    }

    public void ResetShip()
    {
        autopilot = true;
        Position = new Vector2((int)(_screenSize.x * _startPositions[myId - 1].x), _screenSize.y + _bottomPadding);
        _asp_engine.PitchScale = 0.75f;
    }

    public void _on_Area2D_area_entered(Area area)
    {
        PlayEffect("die");
        ResetShip();
    }

    private void PlayEffect(string effect)
    {
        if (effect == "get-point")
        {
            _asp_effect.Stream = (AudioStream)GD.Load("res://Audio/get-point.wav");
        }
        else if (effect == "die")
        {
            _asp_effect.Stream = (AudioStream)GD.Load("res://Audio/die.wav");
        }
        _asp_effect.Play();
    }

    private void _OnTimeOut()
    {
        _timeout = true;
    }

    private void _OnBlinkFinished()
    {
        _timeout = false;
        _firstBoost = false;
    }
}

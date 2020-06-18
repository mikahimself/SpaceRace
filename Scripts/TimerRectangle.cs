using Godot;
using System;

public class TimerRectangle : Node2D
{
    private Vector2 _screenSize;
    private Vector2 _initialRectanglePos;
    private Vector2 _initialRectangleSize;
    private Vector2 _rectanglePos;
    private Vector2 _rectangleSize;
    [Export]
    public int InitialTime = 90;
    private int _timeLeft;
    private bool _updateRect;
    private float _rectPositionFactor = 0.15f;
    private float _rectHeightFactor = 0.85f;
    private int _rectWidth = 10;
    private Timer _tickTimer;
    [Signal]
    public delegate void TimeOut();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _screenSize = GetViewport().Size;
        _initialRectanglePos = new Vector2(_screenSize.x / 2, _screenSize.y * _rectPositionFactor);
        _initialRectangleSize = new Vector2(_rectWidth, _screenSize.y * _rectHeightFactor);
        _timeLeft = InitialTime;
        _rectanglePos = _initialRectanglePos;
        _rectangleSize = _initialRectangleSize;
        _updateRect = false;
        GetParent().GetNode("SpaceShip1").Connect("FirstBoost", this, nameof(_OnFirstBoost));
        GetParent().GetNode("SpaceShip2").Connect("FirstBoost", this, nameof(_OnFirstBoost));
        _tickTimer = (Timer)GetNode("TickTimer");
    }

    public override void _Draw()
    {
        DrawRect(new Rect2(_rectanglePos, _rectangleSize), new Color("#ffffff"));
    }

    public override void _Process(float delta)
    {
        if (_updateRect) {
            Update();
            _updateRect = false;
        }
    }

    private void _OnTimerTimeout()
    {
        _timeLeft -= 1;
        ResizeRectangle();

        if (_timeLeft == 0)
        {
            _tickTimer.Stop();
            EmitSignal(nameof(TimeOut));
        }
    }

    private void _OnFirstBoost()
    {
        if (_tickTimer.IsStopped())
        {
            _tickTimer.Start();
        }
    }

    private void ResizeRectangle()
    {
        float newSizeMultiplier = (1.00f * _timeLeft) / InitialTime;
        var newSize = new Vector2(_rectWidth, _initialRectangleSize.y * newSizeMultiplier);
        var newPos = new Vector2(_initialRectanglePos.x, _screenSize.y * (1.00f - (_rectHeightFactor * newSizeMultiplier)));
        _rectangleSize = newSize;
        _rectanglePos = newPos;
        _updateRect = true;
    }

    private void ResetRectangle()
    {
        _timeLeft = InitialTime;
        _rectanglePos = _initialRectanglePos;
        _rectangleSize = _initialRectangleSize;
        _updateRect = false;
    }

    private void RefillRectangle()
    {
        
    }
}

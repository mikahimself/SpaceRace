using Godot;
using System;

public class ScoreMinder : Node2D
{
    private int[][] _numbers =
    {
        new int[] {1, 1, 1, 1, 1, 0, 1},
        new int[] {0, 0, 1, 1, 0, 0, 0},
        new int[] {0, 1, 1, 0, 1, 1, 1},
        new int[] {0, 0, 1, 1, 1, 1, 1},
        new int[] {1, 0, 1, 1, 0, 1, 0},
        new int[] {1, 0, 0, 1, 1, 1, 1},
        new int[] {1, 1, 0, 1, 0, 1, 1},
        new int[] {0, 0, 1, 1, 1, 0, 0},
        new int[] {1, 1, 1, 1, 1, 1, 1},
        new int[] {1, 0, 1, 1, 1, 1, 0},
        new int[] {0, 0, 0, 0, 0, 0, 0}
    };
    private Vector2 _screenSize;
    private const string _white = "ffffff";
    private const string _black = "000000";
    private float _vertBarWidth;
    private float _vertBarHeight;
    private float _horBarWidth;
    private float _horBarHeight;
    private int _player1Score = 0;
    private int _player2Score = 0;
    private bool _updateScore = false;
    private bool _gameOver = false;
    private Timer _blinkTimer;
    private int _blinkCount = 0;
    private int _noOfBlinks = 10;
    private string _blinkColor;

    [Signal]
    public delegate void OnBlinkFinish();


    public override void _Ready()
    {
        _screenSize = GetViewport().Size;
        _vertBarWidth = _screenSize.x * 0.01f;
        _vertBarHeight = _screenSize.y * 0.06f;
        _horBarHeight = _vertBarWidth;
        _horBarWidth = _vertBarHeight;

        // Get nodes
        _blinkTimer = (Timer)GetNode("BlinkTimer");
        _blinkTimer.Connect("timeout", this, nameof(_OnBlinkTimerTimeout));
    }

    public override void _Process(float delta)
    {
        if (_updateScore) {
            Update();
            _updateScore = false;
        }
    }

    public void SetupSignals()
    {
        // Connect signals from ships and timer
        GetParent().GetNode("SpaceShip1").Connect("GainPoint", this, nameof(_OnGainPoint));
        GetParent().GetNode("SpaceShip2").Connect("GainPoint", this, nameof(_OnGainPoint));
        GetParent().GetNode("TimerRectangle").Connect("TimeOut", this, nameof(_OnTimeOut));
    }

    public override void _Draw()
    {
        if (!_gameOver)
        {
            DrawNumber(_player1Score, 1);
            DrawNumber(_player2Score, 2);
        } 
        else
        {
            if (_player1Score > _player2Score)
            {
                DrawNumber(_player1Score, 1, _blinkColor);
                DrawNumber(_player2Score, 2);
            }
            else if (_player2Score > _player1Score)
            {
                DrawNumber(_player2Score, 2, _blinkColor);
                DrawNumber(_player1Score, 1);
            }
            else
            {
                DrawNumber(_player1Score, 1, _blinkColor);
                DrawNumber(_player2Score, 2, _blinkColor);
            }
        }
    }

    public void DrawNumber(int number, int player, string fontColor = _white)
    {
        Color font = new Color(fontColor);

        Vector2 startPos = player == 1 ? new Vector2(_screenSize.x * 0.25f, _screenSize.y * 0.85f) : new Vector2(_screenSize.x * 0.70f, _screenSize.y * 0.85f);
        Vector2 digitPos = player == 1 ? new Vector2(_screenSize.x * 0.20f, _screenSize.y * 0.85f) : new Vector2(_screenSize.x * 0.65f, _screenSize.y * 0.85f);

        if (number > 9) {
            // Draw number one, and pass the second digit to drawing "properly"
            DrawRect(new Rect2(digitPos.x + _horBarWidth - _vertBarWidth, digitPos.y, _vertBarWidth, _vertBarHeight), font);
            DrawRect(new Rect2(digitPos.x + _horBarWidth - _vertBarWidth, digitPos.y + _vertBarHeight, _vertBarWidth, _vertBarHeight), font);
            number = number % 10;
        }

        if (_numbers[number][0] != 0)
        {
            DrawRect(new Rect2(startPos.x, startPos.y, _vertBarWidth, _vertBarHeight), font);
        };
        if (_numbers[number][1] == 1)
        {
            DrawRect(new Rect2(startPos.x, startPos.y + _vertBarHeight, _vertBarWidth, _vertBarHeight), font);
        }
        if (_numbers[number][2] == 1)
        {
            DrawRect(new Rect2(startPos.x + _horBarWidth - _vertBarWidth, startPos.y, _vertBarWidth, _vertBarHeight), font);
        };
        if (_numbers[number][3] == 1)
        {
            DrawRect(new Rect2(startPos.x + _horBarWidth - _vertBarWidth, startPos.y + _vertBarHeight, _vertBarWidth, _vertBarHeight), font);
        };
        if (_numbers[number][4] != 0)
        {
            DrawRect(new Rect2(startPos.x, startPos.y, _horBarWidth, _horBarHeight), font);
        };
        if (_numbers[number][5] != 0)
        {
            DrawRect(new Rect2(startPos.x, startPos.y + _vertBarHeight - (_horBarHeight / 2), _horBarWidth, _horBarHeight), font);
        };
        if (_numbers[number][6] != 0)
        {
            DrawRect(new Rect2(startPos.x, startPos.y + _vertBarHeight * 2 - _horBarHeight, _horBarWidth, _horBarHeight), font);
        };
    }

    private void _OnGainPoint(int playerID)
    {
        var point = playerID == 1 ? _player1Score++ : _player2Score++;
        _updateScore = true;
    }

    private void _OnTimeOut()
    {
        _blinkTimer.Start();
        _gameOver = true;
    }

    private void _OnBlinkTimerTimeout()
    {
        _blinkCount++;
        _blinkColor = _blinkCount % 2 == 0 ? _white : _black;
        _updateScore = true;
        if (_blinkCount >= _noOfBlinks)
        {
            _blinkTimer.Stop();
            _gameOver = false;
            _player1Score = _player2Score = 0;
            _updateScore = true;
            EmitSignal(nameof(OnBlinkFinish));
            _blinkCount = 0;
        }
    }
}

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
    private Color white = new Color("ffffff");
    private float _vertBarWidth;
    private float _vertBarHeight;
    private float _horBarWidth;
    private float _horBarHeight;
    private int _player1Score = 0;
    private int _player2Score = 0;
    private bool _updateScore = false;

    public override void _Ready()
    {
        _screenSize = GetViewport().Size;
        _vertBarWidth = _screenSize.x * 0.01f;
        _vertBarHeight = _screenSize.y * 0.06f;
        _horBarHeight = _vertBarWidth;
        _horBarWidth = _vertBarHeight;

        // Connect signals from ships
        GetParent().GetNode("SpaceShip1").Connect("GainPoint", this, nameof(_OnGainPoint));
        GetParent().GetNode("SpaceShip2").Connect("GainPoint", this, nameof(_OnGainPoint));
    }

    public override void _Process(float delta)
    {
        if (_updateScore) {
            Update();
            _updateScore = false;
        }
    }

    public override void _Draw()
    {
        DrawNumber(_player1Score, 1);
        DrawNumber(_player2Score, 2);
    }

    public void DrawNumber(int number, int player)
    {
        Vector2 startPos = player == 1 ? new Vector2(_screenSize.x * 0.25f, _screenSize.y * 0.85f) : new Vector2(_screenSize.x * 0.7f, _screenSize.y * 0.85f);

        if (_numbers[number][0] != 0)
        {
            DrawRect(new Rect2(startPos.x, startPos.y, _vertBarWidth, _vertBarHeight), white);
        };
        if (_numbers[number][1] == 1)
        {
            DrawRect(new Rect2(startPos.x, startPos.y + _vertBarHeight, _vertBarWidth, _vertBarHeight), white);
        }
        if (_numbers[number][2] == 1)
        {
            DrawRect(new Rect2(startPos.x + _horBarWidth - _vertBarWidth, startPos.y, _vertBarWidth, _vertBarHeight), white);
        };
        if (_numbers[number][3] == 1)
        {
            DrawRect(new Rect2(startPos.x + _horBarWidth - _vertBarWidth, startPos.y + _vertBarHeight, _vertBarWidth, _vertBarHeight), white);
        };
        if (_numbers[number][4] != 0)
        {
            DrawRect(new Rect2(startPos.x, startPos.y, _horBarWidth, _horBarHeight), white);
        };
        if (_numbers[number][5] != 0)
        {
            DrawRect(new Rect2(startPos.x, startPos.y + _vertBarHeight - (_horBarHeight / 2), _horBarWidth, _horBarHeight), white);
        };
        if (_numbers[number][6] != 0)
        {
            DrawRect(new Rect2(startPos.x, startPos.y + _vertBarHeight * 2 - _horBarHeight, _horBarWidth, _horBarHeight), white);
        };
    }

    private void _OnGainPoint(int playerID)
    {
        var point = playerID == 1 ? _player1Score++ : _player2Score++;
        _updateScore = true;
    }
}

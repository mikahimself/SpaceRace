using Godot;
using System;

public class MenuScreen : Control
{
    private SceneChanger _sceneChanger;
    private bool _showMainMenu;
    private Timer _menuTimer;
    [Export]
    public int ShowMenuTime;
    [Export]
    public int ShowInfoTime;
    private VBoxContainer _mainText;
    private VBoxContainer _infoText;

    public override void _Ready()
    {
        _sceneChanger = (SceneChanger)GetNode("/root/SceneChanger");
        _menuTimer = (Timer)GetNode("MenuTimer");
        _mainText = (VBoxContainer)GetNode("MenuPanel/MainText");
        _infoText = (VBoxContainer)GetNode("MenuPanel/InfoText");
        _sceneChanger.revealScreen();
        _showMainMenu = true;
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionPressed("ui_accept"))
        {
            _sceneChanger.changeScene("res://Screens/GameScreen.tscn", 0.5f);
        }
    }

    private void _on_MenuTimer_timeout()
    {
        if (_showMainMenu)
        {
            _menuTimer.Start(ShowInfoTime);
            _mainText.Visible = false;
            _infoText.Visible = true;
            _showMainMenu = false;
        }
        else
        {
            _menuTimer.Start(ShowMenuTime);
            _infoText.Visible = false;
            _mainText.Visible = true;
            _showMainMenu = true;
        }
    }
}

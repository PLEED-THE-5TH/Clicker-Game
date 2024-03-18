using Godot;
using System;

public partial class PauseMenu : CanvasLayer
{
    Button continueButton;
    Button mainMenuButton;
    Button settingsButton;
    Button exitButton;
    public override void _Ready()
    {
        continueButton = GetNode<Button>("CenterContainer/Control/Panel/VBoxContainer/Continue");
        mainMenuButton = GetNode<Button>("CenterContainer/Control/Panel/VBoxContainer/Main Menu");
        settingsButton = GetNode<Button>("CenterContainer/Control/Panel/VBoxContainer/Settings");
        exitButton = GetNode<Button>("CenterContainer/Control/Panel/VBoxContainer/Exit");
        continueButton.Pressed += ContinueButtonPressed;
        mainMenuButton.Pressed += MainMenuButtonPressed;
        settingsButton.Pressed += SettingsButtonPressed;
        exitButton.Pressed += ExitButtonPressed;
    }
    private void ContinueButtonPressed()
    {
        World.ClosePauseMenu();
    }
    private void MainMenuButtonPressed()
    {
        GetTree().ChangeSceneToPacked(GD.Load<PackedScene>("res://Scenes/World/MainMenu.tscn"));
    }
    private void SettingsButtonPressed()
    {

    }
    private void ExitButtonPressed()
    {
        GetTree().Quit();
    }
}
